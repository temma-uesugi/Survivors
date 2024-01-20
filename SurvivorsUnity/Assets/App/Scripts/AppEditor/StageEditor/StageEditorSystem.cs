#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using App.AppEditor.StageEditor.Records;
using App.AppEditor.StageEditor.UIElements;
using UniRx;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// BotNodeEditorのExtension
    /// </summary>
    public class StageEditorSystem : IDisposable
    {
        private readonly CompositeDisposable disposable = new();
        private static StageEditorSystem instance;
        public static StageEditorSystem Instance => instance ??= new StageEditorSystem();

        //NodeTreeView
        private StageEditorWindow window = null;
        public StageEditorInspectorWindow Inspector { get; private set; } = null;

        public uint GridSize { get; private set; }

        private GridPositionIcon positionIconManager;
        private readonly ItemDataManager itemDataManager = new ItemDataManager();
        private readonly PresetSettingManager presetManager = new PresetSettingManager();
        private ScaleManger scaler;

        public StageEditorSetting StageSetting { get; private set; }
        private int initPosX = 1;
        private int initPosY = 1;
        private float scrollerWidth;
        private float scrollerHeight;

        public float ViewScale { get; private set; } = 1;
        private IDisposable editDisposable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private StageEditorSystem()
        {
            EditorSceneManager.sceneClosed += (scene) =>
            {
                if (scene == EditorSceneManager.GetActiveScene())
                {
                    Dispose();
                }
            };
        }

        /// <summary>
        /// 新規作成
        /// </summary>
        public void CreateNewStage(StageEditorSetting setting, uint gridSize)
        {
            StageSetting = setting;
            GridSize = gridSize;
            if (window == null)
            {
                InitView();
            }
            if (Inspector == null)
            {
                InitInspector();
            }
            window.RenderView(setting.Width, setting.Height, gridSize, setting.QuestId, setting.StageNo, setting.QuestStageId);
            Inspector.Reset();
            initPosX = 1;
            initPosY = 1;
        }

        /// <summary>
        /// ステージ編集
        /// </summary>
        public void EditStage(StageEditorSetting setting, uint gridSize, int initX, int initY, ItemSaveData[] objects, ItemSaveData[] enemies)
        {
            CreateNewStage(setting, gridSize);
            editDisposable = window.OnRenderCompleted.Subscribe(_ => SetupEditItems());

            //編集用の初期化
            void SetupEditItems()
            {
                initPosX = initX;
                initPosY = initY;
                window.IconLayer.UpdateInitPosition(initX, initY);
                foreach (var obj in objects)
                {
                    var settingData = StageEditorUtil.JsonToSettingData(obj.DataJson);
                    if (!presetManager.TryGet(settingData.PresetId, out var record))
                    {
                        record = StageEditorUtil.SettingDataToRecord(settingData);
                    }
                    AddItem(obj.X, obj.Y, record);
                }
                foreach (var enemy in enemies)
                {
                    var settingData = StageEditorUtil.JsonToSettingData(enemy.DataJson);
                    if (!presetManager.TryGet(settingData.PresetId, out var record))
                    {
                        record = StageEditorUtil.SettingDataToRecord(settingData);
                    }
                    AddItem(enemy.X, enemy.Y, record);
                }
                editDisposable.Dispose();
            }
        }

        /// <summary>
        /// Viewの初期化
        /// </summary>
        private void InitView()
        {
            window = EditorWindow.GetWindow<StageEditorWindow>();
            window.OnGridClick.Subscribe(grid => OnGridClick(grid.x, grid.y)).AddTo(disposable);
            window.OnKeyDown //
                .Where(e => e == KeyCode.Space && positionIconManager != null) //
                .Subscribe(_ => OnGridClick(positionIconManager.CurrentX, positionIconManager.CurrentY)) //
                .AddTo(disposable);
            window.OnKeyDown //
                .Where(e => e == KeyCode.Escape) //
                .Subscribe(_ => window.GridInfo.Hide()) //
                .AddTo(disposable);

            window.OnRenderCompleted.Subscribe(_ => ViewRenderCompleted()).AddTo(disposable);
        }

        /// <summary>
        /// Viewの描画完了
        /// </summary>
        private void ViewRenderCompleted()
        {
            positionIconManager = new GridPositionIcon(window);
            itemDataManager.Reset();

            window.GridInfo.OnAddItem.Subscribe(val => AddItem(val.x, val.y)).AddTo(disposable);
            window.GridInfo.OnRemoveAll.Subscribe(val => RemoveGridItems(val.x, val.y)).AddTo(disposable);
            window.GridInfo.OnRemoveItem.Subscribe(RemoveItem).AddTo(disposable);
            window.GridInfo.OnSelectedItem.Subscribe(SelectItem).AddTo(disposable);
            window.GridInfo.OnOpened.Subscribe(_ => positionIconManager.Lock()).AddTo(disposable);
            window.GridInfo.OnClosed.Subscribe(_ =>
            {
                Inspector.UnsetByGrindInfoClosed();
                positionIconManager.Unlock();
            }).AddTo(disposable);
            window.ItemList.OnRemoveItem.Subscribe(RemoveItem).AddTo(disposable);
            window.ItemList.OnSelectedItem.Subscribe(itemData =>
            {
                //TODO 移動しない なぜ？
                positionIconManager.MoveToGrid(itemData.X, itemData.Y);
                SelectItem(itemData);
            }).AddTo(disposable);

            foreach (Define.SettingDataType t in Enum.GetValues(typeof(Define.SettingDataType)))
            {
                window.Selector.AddItem(t.GetDefaultRecord());
            }
            foreach (var presetRec in presetManager.AllRecords)
            {
                window.Selector.AddItem(presetRec);
            }
            window.Selector.OnModeChanged.Subscribe(mode =>
            {
                window.SetModeLabel(mode, window.Selector.CurrentRecord);
                window.GridMap.Focus();
            }).AddTo(disposable);

            window.IconLayer.UpdateInitPosition(initPosX, initPosY);
            scaler = new ScaleManger(window);
            scaler.OnScaleChanged
                .Subscribe(ScaleChanged)
                .AddTo(disposable);

            scrollerWidth = window.GridMapScroller.contentRect.width;
            scrollerHeight = window.GridMapScroller.contentRect.height;

            //scaleを変化させた時のPivotを設定
            window.GridLayer.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length(0), new Length(0)));
            window.IconLayer.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length(0), new Length(0)));
            window.GridPosition.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(new Length(0), new Length(0)));
        }

        /// <summary>
        /// インスペクターの初期化
        /// </summary>
        private void InitInspector()
        {
            Inspector = EditorWindow.GetWindow<StageEditorInspectorWindow>();
            Inspector.OnEditFinished.Subscribe(EditFinish).AddTo(disposable);
            Inspector.OnSetPreset.Subscribe(x =>
            {
                SetPreset(x.id, x.record);
            }).AddTo(disposable);
            Inspector.OnDeletePreset.Subscribe(DeletePreset).AddTo(disposable);
            Inspector.OnUnlinkPreset.Subscribe(UnlinkPreset).AddTo(disposable);
            Inspector.OnAddItem.Subscribe(val =>
            {
                window.GridPosition.MoveToGrid(val.x, val.y);
                AddItem(val.x, val.y, val.record);
            }).AddTo(disposable);
        }

        /// <summary>
        /// Gridをクリック
        /// </summary>
        private void OnGridClick(int x, int y)
        {
            window.GridInfo.Hide();
            if (window.Selector.Mode == Define.ModeType.Remove)
            {
                RemoveGridItems(x, y);
            }
            else if (window.Selector.Mode == Define.ModeType.InitPos)
            {
                initPosX = x;
                initPosY = y;
                window.IconLayer.UpdateInitPosition(x, y);
            }
            else
            {
                var gridItemList = itemDataManager.GetAllByGrid(x, y);
                if (gridItemList.Any())
                {
                    SelectGrid(x, y, gridItemList);
                }
                else if (window.Selector.Mode == Define.ModeType.Add)
                {
                    AddItem(x, y);
                }
            }
        }

        /// <summary>
        /// Item追加
        /// </summary>
        private void AddItem(int x, int y) => AddItem(x, y, window.Selector.CurrentRecord);

        /// <summary>
        /// Item追加
        /// </summary>
        private void AddItem(int x, int y, SettingRecord record)
        {
            window.GridInfo.Hide();
            var itemData = itemDataManager.AddItem(record, x, y);
            window.ItemList.AddItem(itemData);
            window.IconLayer.AddItem(itemData.Id, itemData.Record, x, y);
        }

        /// <summary>
        /// グリッドの選択
        /// </summary>
        private void SelectGrid(int x, int y, List<ItemData> itemDataList)
        {
            window.GridInfo.Hide();
            window.GridInfo.Show(x, y, itemDataList, window.Selector.Mode == Define.ModeType.Add);
        }

        /// <summary>
        /// グリッドの全Item除去
        /// </summary>
        private void RemoveGridItems(int x, int y)
        {
            window.GridInfo.Hide();
            var removedIdList = itemDataManager.RemoveGridAll(x, y);
            foreach (var itemData in removedIdList)
            {
                window.IconLayer.RemoveItem(itemData.Id);
                window.ItemList.RemoveItem(itemData.Id);
            }
        }

        /// <summary>
        /// アイテム除去
        /// </summary>
        private void RemoveItem(int id)
        {
            window.GridInfo.Hide();
            itemDataManager.Remove(id);
            window.ItemList.RemoveItem(id);
            window.IconLayer.RemoveItem(id);
        }

        /// <summary>
        /// アイテム選択
        /// </summary>
        private void SelectItem(ItemData itemData)
        {
            //Note: この処理は不要そうなのでコメントアウト
            // View.Selector.SetSelectMode();
            Inspector.SetEditRecord(itemData.Id, itemData.Record);
        }

        /// <summary>
        /// 編集レコードの終了
        /// </summary>
        private void EditFinish(SettingRecord record)
        {
            window.IconLayer.Update(record);
            window.Selector.UpdateItem(record);
            if (record.IsPreset)
            {
                presetManager.UpdatePreset(record);
            }
            Inspector.Unset();
            window.GridInfo.Hide();
            //Note: Focusされない??
            window.GridMap.Focus();
        }

        /// <summary>
        /// プリセットデータを登録
        /// </summary>
        private void SetPreset(int itemId, SettingRecord record)
        {
            var presetRecord = presetManager.SetPreset(record);
            window.Selector.AddItem(presetRecord);
            if (itemId != Define.InvalidItemId)
            {
                //既存のアイテムをプリセットに置き換え
                itemDataManager.SwitchPresetRecord(itemId, presetRecord);
                //TODO: ItemListの見た目を変える処理が必要
                window.IconLayer.ChangeRecord(itemId, presetRecord);
            }
            window.GridInfo.Hide();
            window.Selector.SetSelectItem(presetRecord);
            //Note: Focusされない??
            window.GridMap.Focus();
        }

        /// <summary>
        /// プリセットデータの削除
        /// </summary>
        private void DeletePreset(SettingRecord record)
        {
            window.Selector.RemoveItem(record);
            presetManager.RemovePreset(record);
            var targets = itemDataManager.UnlinkPresetAll(record);
            foreach (var target in targets)
            {
                window.IconLayer.ChangeRecord(target.id, target.record);
                //TODO: ItemListの見た目を変える処理が必要
            }
            window.GridInfo.Hide();
            Instance.Inspector.Unset();
            window.Selector.SetSelectMode();
        }

        /// <summary>
        /// プリセットのリンク解除
        /// </summary>
        private void UnlinkPreset(int itemId)
        {
            var cloneRecord = itemDataManager.UnlinkPreset(itemId);
            window.IconLayer.ChangeRecord(itemId, cloneRecord);
            window.GridInfo.Hide();
            //TODO: ItemListの見た目を変える処理が必要
        }

        /// <summary>
        /// SQL吐き出し
        /// </summary>
        public void SaveAndExportSql()
        {
            var items = itemDataManager.AllItem.ToArray();
            DataFileManager.Save(StageSetting, initPosX, initPosY, items);
            var insertBuilder = SqlExporter.CreateInsertSql(StageSetting, initPosX, initPosY, items);
            var presetBuilder = SqlExporter.CreateUpdatePresetSql(presetManager.UpdatedRecords, presetManager.RemovedRecords);
            window.SqlDisplay.SetText(insertBuilder, presetBuilder);
            presetManager.SaveAndReset();
        }

        /// <summary>
        /// Presetによる更新処理SQLの吐き出し
        /// </summary>
        public void ExportPresetSql()
        {
            if (presetManager.UpdatedRecords.Any())
            {
                EditorUtility.DisplayDialog("", "更新が確定されていないPresetデータがあります。先に確定させてください", "OK");
                return;
            }
            if (!presetManager.AllRecords.Any())
            {
                EditorUtility.DisplayDialog("", "Presetデータが存在しません", "OK");
                return;
            }
            var builder = SqlExporter.CreateUpdatePresetSql(presetManager.AllRecords);
            window.SqlDisplay.SetText(builder);
        }

        /// <summary>
        /// スケール変更
        /// </summary>
        private void ScaleChanged(float scale)
        {
            ViewScale = scale;
            window.GridLayer.style.scale = new StyleScale(new Scale(new Vector3(scale, scale, scale)));
            window.IconLayer.style.scale = new StyleScale(new Scale(new Vector3(scale, scale, scale)));
            window.GridPosition.style.scale = new StyleScale(new Scale(new Vector3(scale, scale, scale)));
            //Note: スクロールの調整。スクローラの子要素(全て?)のサイズを変える必要がありそう？うまく行かなかったので保留gg
            // View.GridMap.parent.style.width = new StyleLength(scrollerWidth * fScale);
            // View.GridMap.parent.style.height = new StyleLength(scrollerHeight * fScale);
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            if (window != null)
            {
                window.Clear();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Debug.Log("StageEditorSystem Dispose");
            disposable.Dispose();
            editDisposable?.Dispose();
        }
    }
}
#endif
