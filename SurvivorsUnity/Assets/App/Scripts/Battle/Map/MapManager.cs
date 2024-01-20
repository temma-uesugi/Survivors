﻿using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Core;
using UnityEngine;

namespace App.Battle.Map
{
    /// <summary>
    /// Map管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(MapManager))]
    public class MapManager : MonoBehaviour
    {
        private int _xAmount;
        private int _yAmount;

        public int Width => _xAmount;
        public int Height => _yAmount;
       
        private readonly HashSet<HexCell> _cells = new();
        private HexCell[,] _mapCells;
        public HexCell[] AllMapCells { get; private set; }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int xAmount, int yAmount)
        {
            _xAmount = xAmount;
            _yAmount = yAmount;
            
            //Mapセル作成
            _mapCells = new HexCell[_yAmount, _xAmount];
            for (int y = 0; y < _yAmount; y++)
            {
                for (int x = 0; x < _xAmount; x++)
                {
                    var cell = CreateMapCell(x, y);
                    _cells.Add(cell);
                    _mapCells[y, x] = cell;
                }
            }
            AllMapCells = _mapCells.Cast<HexCell>().ToArray();
            
            //周りに侵入不可セル
            foreach (var (x, y) in GetAroundNoEnterCellList(xAmount, yAmount))
            {
                var cell = CreateNoEnterCell(x, y);
                _cells.Add(cell);
            }
        }
        
        /// <summary>
        /// 海セルの作成
        /// </summary>
        private HexCell CreateMapCell(int x, int y)
        {
            var pos = HexUtil.HexPosToLocalPos(x, y);
            var prefab = ResourceMaps.Instance.CellPrefab.GetObject(MapCellType.Normal);
            var cell = Instantiate(prefab, transform);
            cell.transform.localPosition = pos;
            cell.Setup(x, y);
            return cell;
        }

        /// <summary>
        /// 侵入不可エリアの作成
        /// </summary>
        private NoEnterCell CreateNoEnterCell(int x, int y)
        {
            var pos = HexUtil.HexPosToLocalPos(x, y);
            var prefab = ResourceMaps.Instance.CellPrefab.GetObject(MapCellType.NoEnter) as NoEnterCell;
            var cell = Instantiate(prefab, transform);
            cell.transform.localPosition = pos;
            cell.Setup(x, y);
            return cell;
        }
        
        /// <summary>
        /// 周囲の侵入不可エリア
        /// </summary>
        private List<(int x, int y)> GetAroundNoEnterCellList(int xAmount, int yAmount)
        {
            var list = new List<(int x, int y)>();
            for (int i = -1; i < yAmount + 1; i++)
            {
                list.Add((-1, i));
            }
            //右端
            for (int i = -1; i < yAmount + 1; i++)
            {
                list.Add((xAmount, i));
            }
            //下
            for (int i = 0; i < xAmount; i++)
            {
                list.Add((i, -1));
            }
            //上
            for (int i = 0; i < xAmount; i++)
            {
                list.Add((i, yAmount));
            }
            return list;
        }
    }
}