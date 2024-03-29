﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Ink;

namespace DAL
{
    /// <summary>
    /// 识别核心类
    /// </summary>
    public class RecognizationCore : INotifyPropertyChanged
    {
        #region SelectedRecognizer

        private ICharactorRecognizer _selectedRecoginzer;

        /// <summary>
        /// Get/Set 识别器
        /// </summary>
        public ICharactorRecognizer SelectedRecognizer
        {
            get { return _selectedRecoginzer; }
            set
            {
                if (_selectedRecoginzer != value)
                {
                    _selectedRecoginzer = value;
                    this.Alternates = Constants.EmptyAlternates;
                    NotifyPropertyChanged("SelectedRecognizer");
                }
            }
        }

        #endregion

        #region Alternates

        private IEnumerable<string> _alternates;

        /// <summary>
        /// Get 候选词列表
        /// </summary>
        public IEnumerable<string> Alternates
        {
            get { return _alternates; }
            private set
            {
                if (_alternates != value)
                {
                    _alternates = value;
                    NotifyPropertyChanged("Alternates");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// 属性变化时触发
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// 创建一个识别核心类
        /// </summary>

		public RecognizationCore(int lang )
		{
			this.SelectedRecognizer = new ImprovedRecognizer(lang);
		}

        /// <summary>
        /// 进行识别
        /// </summary>
        /// <param name="strokes">笔迹集合</param>
        public void Recognize(StrokeCollection strokes)
        {
            if (this.SelectedRecognizer == null)
                return;

            this.Alternates = this.SelectedRecognizer.Recognize(strokes);
        }

        /// <summary>
        /// 清空候选词
        /// </summary>
        public void ClearAlternates()
        {
            this.Alternates = Constants.EmptyAlternates;
        }
    }
}
