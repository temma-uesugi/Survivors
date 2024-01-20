#if UNITY_EDITOR
using System;
using App.AppCommon;
using App.AppCommon.Core;
using App.AppEditor.StageEditor.Records;
using UniRx;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// StageEditor用インスペクター
    /// </summary>
    public class StageEditorInspectorWindow : EditorWindow
    {
        private VisualElement root;
        private Box objectSetting;
        private Box enemySetting;

        //各要素
        private Label statusLabel;
        private TextField itemLabel;
        private ColorField colorField;
        private TextField itemDesc;
        private IntegerField objectParam1;
        private IntegerField objectGetPoint;
        private IntegerField enemyParam1;
        private IntegerField enemyParam2;
        private IntegerField enemyLevel;
        private IntegerField enemySpawnTime;
        private IntegerField enemyGetPoint;
        private Toggle enemyIsBoss;
        private Toggle enemyIsStealth;
        private IntegerField positionX;
        private IntegerField positionY;

        private bool isOnEdit = false;
        //即時反映OKか
        private bool isImmedUpdate => !isOnEdit && !currentRecord.IsPreset;
        private SettingRecord currentRecord;
        private int currentItemId;

        private Button editFinishBtn;
        private Button setPresetBtn;
        private Button editFinishPresetBtn;
        private Button deletePresetBtn;
        private Button unlinkPresetBtn;
        private Button addItemBtn;

        private Box addToPositionBox;

        private readonly Subject<SettingRecord> onEditFinished = new();
        public IObservable<SettingRecord> OnEditFinished => onEditFinished;
        private readonly Subject<(int id, SettingRecord record)> onSetPreset = new();
        public IObservable<(int id, SettingRecord record)> OnSetPreset => onSetPreset;
        private readonly Subject<SettingRecord> onDeletePreset = new ();
        public IObservable<SettingRecord> OnDeletePreset => onDeletePreset;
        private readonly Subject<int> onUnlinkPreset = new();
        public IObservable<int> OnUnlinkPreset => onUnlinkPreset;
        private readonly Subject<(SettingRecord record, int x, int y)> onAddItem = new();
        public IObservable<(SettingRecord record, int x, int y)> OnAddItem => onAddItem;

        /// <summary>
        /// CreateGUI
        /// </summary>
        public void CreateGUI()
        {
            Log.Debug("CrateGUI");
            root = rootVisualElement;
            // Import UXML
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{StageEditorWindow.SourceDir}/StageEditorInspectorWindow.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/StageEditorInspectorWindow.uss");
            var visualTree = visualTreeAsset.Instantiate();
            visualTree.styleSheets.Add(styleSheet);
            root.Add(visualTree);

            objectSetting = root.Q<Box>("objectSetting");
            enemySetting = root.Q<Box>("enemySetting");

            statusLabel = root.Q<Label>("editStatus");
            itemLabel = root.Q<TextField>("itemLabel");
            colorField = root.Q<ColorField>();
            itemDesc = root.Q<TextField>("itemDesc");
            objectParam1 = objectSetting.Q<IntegerField>("Object-ObjectParam1");
            objectGetPoint = objectSetting.Q<IntegerField>("Object-GetPoint");
            enemyParam1 = enemySetting.Q<IntegerField>("Enemy-EnemyParam1");
            enemyParam2 = enemySetting.Q<IntegerField>("Enemy-EnemyParam2");
            enemyLevel = enemySetting.Q<IntegerField>("Enemy-Level");
            enemySpawnTime = enemySetting.Q<IntegerField>("Enemy-SpawnGameTime");
            enemyGetPoint = enemySetting.Q<IntegerField>("Enemy-GetPoint");
            enemyIsBoss = enemySetting.Q<Toggle>("Enemy-IsBoss");
            enemyIsStealth = enemySetting.Q<Toggle>("Enemy-IsStealth");
            editFinishBtn = root.Q<Button>("editFinishBtn");
            setPresetBtn = root.Q<Button>("setPresetBtn");
            editFinishPresetBtn = root.Q<Button>("editFinishPresetBtn");
            deletePresetBtn = root.Q<Button>("deletePresetBtn");
            unlinkPresetBtn = root.Q<Button>("unlinkPresetBtn");

            addToPositionBox = root.Q<Box>("addToPosition");
            positionX = root.Q<IntegerField>("positionX");
            positionY = root.Q<IntegerField>("positionY");
            addItemBtn = root.Q<Button>("addItemBtn");

            //Fieldの編集をRecordに反映する処理
            itemLabel.RegisterValueChangedCallback(e =>
            {
                //編集中は編集確定してから反映する
                if (currentRecord != null && isImmedUpdate)
                {
                    currentRecord.ItemLabel = e.newValue;
                }
            });
            colorField.RegisterValueChangedCallback(e =>
            {
                if (currentRecord != null && isImmedUpdate)
                {
                    currentRecord.IconColor = e.newValue;
                }
            });
            itemDesc.RegisterValueChangedCallback(e =>
            {
                if (currentRecord != null && isImmedUpdate)
                {
                    currentRecord.ItemDesc = e.newValue;
                }
            });
            objectParam1.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is ObjectRecord objectRecord && isImmedUpdate)
                {
                    objectRecord.ObjectParam1 = e.newValue;
                }
            });
            objectGetPoint.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is ObjectRecord objectRecord && isImmedUpdate)
                {
                    objectRecord.GetPoint = e.newValue;
                }
            });
            enemyParam1.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.EnemyParam1 = e.newValue;
                }
            });
            enemyParam2.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.EnemyParam2 = e.newValue;
                }
            });
            enemyLevel.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.Level = e.newValue;
                }
            });
            enemySpawnTime.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.SpawnGameTime = e.newValue;
                }
            });
            enemyGetPoint.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.GetPoint = e.newValue;
                }
            });
            enemyIsBoss.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.IsBoss = e.newValue;
                }
            });
            enemyIsStealth.RegisterValueChangedCallback(e =>
            {
                if (currentRecord is EnemyRecord enemyRecord && isImmedUpdate)
                {
                    enemyRecord.IsStealth = e.newValue;
                }
            });
            editFinishBtn.clickable.clicked += EditFinish;
            setPresetBtn.clickable.clicked += SetPreset;
            editFinishPresetBtn.clickable.clicked += EditFinish;
            deletePresetBtn.clickable.clicked += DeletePreset;
            unlinkPresetBtn.clickable.clicked += UnlinkPreset;
            addItemBtn.clickable.clicked += AddItemToPosition;
        }

        /// <summary>
        /// 追加レコードをセット
        /// </summary>
        public void SetAddRecord(SettingRecord record)
        {
            //Presetの場合は編集モードに
            isOnEdit = false;
            currentItemId = Define.InvalidItemId;
            UpdateField(record, false);
            SwitchButtonDisplay(record);
        }

        /// <summary>
        /// 編集レコードをセット
        /// </summary>
        public void SetEditRecord(int itemId, SettingRecord record)
        {
            isOnEdit = true;
            currentItemId = itemId;
            UpdateField(record, true);
            SwitchButtonDisplay(record);
        }

        /// <summary>
        /// Field値を更新
        /// </summary>
        private void UpdateField(SettingRecord record, bool isEdit)
        {
            statusLabel.text = isEdit ? "編集" : "追加";
            if (record.IsPreset)
            {
                statusLabel.text += "(プリセット)";
            }
            itemLabel.value = record.ItemLabel;
            colorField.value = record.IconColor;
            itemDesc.value = record.ItemDesc;
            if (record is ObjectRecord objectRecord)
            {
                objectSetting.RemoveFromClassList("hidden");
                enemySetting.AddToClassList("hidden");
                ObjectRecordToInput(objectRecord);
            }
            else if (record is EnemyRecord enemyRecord)
            {
                objectSetting.AddToClassList("hidden");
                enemySetting.RemoveFromClassList("hidden");
                EnemyRecordToInput(enemyRecord);
            }
        }

        /// <summary>
        /// GridInfoが閉じたことによるUnset
        /// </summary>
        public void UnsetByGrindInfoClosed()
        {
            if (!isOnEdit)
            {
                return;
            }
            Unset();
        }

        /// <summary>
        /// Unset
        /// </summary>
        public void Unset()
        {
            objectSetting.AddToClassList("hidden");
            enemySetting.AddToClassList("hidden");
            editFinishBtn.AddToClassList("hidden");
            setPresetBtn.AddToClassList("hidden");
            editFinishPresetBtn.AddToClassList("hidden");
            deletePresetBtn.AddToClassList("hidden");
            unlinkPresetBtn.AddToClassList("hidden");
            itemLabel.value = "";
            colorField.value = Color.white;
            statusLabel.text = "";
            itemDesc.value = "";
            positionX.value = 1;
            positionY.value = 1;
            currentRecord = null;
        }

        /// <summary>
        /// Objectレコードの反映
        /// </summary>
        private void ObjectRecordToInput(ObjectRecord objectRecord)
        {
            currentRecord = objectRecord;
            objectParam1.SetValueWithoutNotify(objectRecord.ObjectParam1);
            objectGetPoint.SetValueWithoutNotify(objectRecord.GetPoint);
        }

        /// <summary>
        /// Enemyレコードの反映
        /// </summary>
        private void EnemyRecordToInput(EnemyRecord enemyRecord)
        {
            currentRecord = enemyRecord;
            enemyParam1.SetValueWithoutNotify(enemyRecord.EnemyParam1);
            enemyParam2.SetValueWithoutNotify(enemyRecord.EnemyParam2);
            enemyLevel.SetValueWithoutNotify(enemyRecord.Level);
            enemySpawnTime.SetValueWithoutNotify(enemyRecord.SpawnGameTime);
            enemyGetPoint.SetValueWithoutNotify(enemyRecord.GetPoint);
            enemyIsBoss.SetValueWithoutNotify(enemyRecord.IsBoss);
            enemyIsStealth.SetValueWithoutNotify(enemyRecord.IsStealth);
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            currentRecord = null;
        }

        /// <summary>
        /// 編集終了
        /// </summary>
        private void EditFinish()
        {
            UpdateRecord();
            onEditFinished.OnNext(currentRecord);
        }

        /// <summary>
        /// プリセットの登録
        /// </summary>
        private void SetPreset()
        {
            UpdateRecord();
            onSetPreset.OnNext((currentItemId, currentRecord));
        }

        /// <summary>
        /// レコードのUpdate
        /// </summary>
        private void UpdateRecord()
        {
            currentRecord.ItemLabel = itemLabel.value;
            currentRecord.IconColor = colorField.value;
            currentRecord.ItemDesc = itemDesc.value;
            if (currentRecord is ObjectRecord objectRecord)
            {
                objectRecord.ObjectParam1 = objectParam1.value;
                objectRecord.GetPoint = objectGetPoint.value;
            }
            else if (currentRecord is EnemyRecord enemyRecord)
            {
                enemyRecord.EnemyParam1 = enemyParam1.value;
                enemyRecord.EnemyParam2 = enemyParam2.value;
                enemyRecord.Level = enemyLevel.value;
                enemyRecord.SpawnGameTime = enemySpawnTime.value;
                enemyRecord.GetPoint = enemyGetPoint.value;
                enemyRecord.IsBoss = enemyIsBoss.value;
                enemyRecord.IsStealth = enemyIsStealth.value;
            }
        }

        /// <summary>
        /// プリセットの削除
        /// </summary>
        private void DeletePreset()
        {
            onDeletePreset.OnNext(currentRecord);
        }

        /// <summary>
        /// プリセットのリンク解除
        /// </summary>
        private void UnlinkPreset()
        {
            onUnlinkPreset.OnNext(currentItemId);
        }

        /// <summary>
        /// ボタンの表示・非表示の切り替え
        /// </summary>
        private void SwitchButtonDisplay(SettingRecord record)
        {
            //一旦全て非表示
            editFinishBtn.AddToClassList("hidden");
            setPresetBtn.AddToClassList("hidden");
            editFinishPresetBtn.AddToClassList("hidden");
            deletePresetBtn.AddToClassList("hidden");
            unlinkPresetBtn.AddToClassList("hidden");
            addToPositionBox.AddToClassList("hidden");
            if (isOnEdit && !record.IsPreset)
            {
                //編集かつプリセットではない
                editFinishBtn.RemoveFromClassList("hidden");
                setPresetBtn.RemoveFromClassList("hidden");
            }
            else if (isOnEdit && record.IsPreset)
            {
                //編集かつプリセット
                editFinishPresetBtn.RemoveFromClassList("hidden");
                unlinkPresetBtn.RemoveFromClassList("hidden");
            }
            else if (!isOnEdit && !record.IsPreset)
            {
                //追加かつプリセットではない
                setPresetBtn.RemoveFromClassList("hidden");
                addToPositionBox.RemoveFromClassList("hidden");
            }
            else if (!isOnEdit && record.IsPreset)
            {
                //追加かつプリセット
                editFinishPresetBtn.RemoveFromClassList("hidden");
                deletePresetBtn.RemoveFromClassList("hidden");
                addToPositionBox.RemoveFromClassList("hidden");
            }
        }

        /// <summary>
        /// ポジションを指定して追加
        /// </summary>
        private void AddItemToPosition()
        {
            var x = Math.Clamp(positionX.value, 1, (int)StageEditorSystem.Instance.StageSetting.Width);
            var y = Math.Clamp(positionY.value, 1, (int)StageEditorSystem.Instance.StageSetting.Height);
            onAddItem.OnNext((currentRecord, x, y));
        }

        /// <summary>
        /// 閉じた
        /// </summary>
        private void OnDestroy()
        {
        }
    }
}
#endif
