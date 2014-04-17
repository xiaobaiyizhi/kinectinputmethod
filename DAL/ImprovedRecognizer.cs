using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Input;

namespace DAL
{
    /// <summary>
    /// 改进的识别器
    /// </summary>
    public class ImprovedRecognizer : ICharactorRecognizer
    {

		private int lang;//识别语言
        /// <summary>
        /// Get 识别器名称
        /// </summary>
        public string Name
        {
            get { return "改进的识别器"; }
        }

		public ImprovedRecognizer(int lang)
		{
			this.lang = lang;
		}
        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="strokes">笔迹集合</param>
        /// <returns>候选词数组</returns>
        public string[] Recognize(StrokeCollection strokes)
        {
            if (strokes == null || strokes.Count == 0)
                return Constants.EmptyAlternates;

            var stroke = GetCombinedStore(strokes);

            var analyzer = new InkAnalyzer();
            analyzer.AddStroke(stroke, lang);
            analyzer.SetStrokeType(stroke, StrokeType.Writing);

            var status = analyzer.Analyze();
            if (status.Successful)
            {
                return analyzer.GetAlternates()
                    .OfType<AnalysisAlternate>()
                    .Select(x => x.RecognizedString)
                    .ToArray();
            }

            analyzer.Dispose();

            return Constants.EmptyAlternates;
        }

        private static Stroke GetCombinedStore(StrokeCollection strokes)
        {
            var points = new StylusPointCollection();
            foreach (var stroke in strokes)
            {
                points.Add(stroke.StylusPoints);
            }
            return new Stroke(points);
        }
    }
}
