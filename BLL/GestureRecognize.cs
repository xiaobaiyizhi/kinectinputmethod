using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BLL
{
	public class GestureRecognize
	{
		/// <summary>
		/// InkCanvas动作识别
		/// </summary>
		/// <param name="gestureResults">e</param>
		/// <returns>操作指令</returns>
		public static string InkGestureResult(ReadOnlyCollection<GestureRecognitionResult> gestureResults)
		{
			if (gestureResults[0].RecognitionConfidence != RecognitionConfidence.Strong)
			{
				return null;
			}
			else
			{
				switch (gestureResults[0].ApplicationGesture)
                {
                    case ApplicationGesture.Down:
                        
						return "space";
                    case ApplicationGesture.ArrowDown:

						return "arrowdwon";
                    case ApplicationGesture.Right:
						return "circle";
                    case ApplicationGesture.Left:
						return "backspace";
					default :
							return "undefined";
                }
			}
		}
		
		/// <summary>
		/// 字体识别
		/// </summary>
		/// <param name="strokes">InkCanvas墨迹</param>
		/// <returns>string接口</returns>
		public static IEnumerable<string> CharactersResult(StrokeCollection strokes)
		{
			return CharactersResult(strokes, Models.Lang.ChsLanguageId);
		}

		public static IEnumerable<string> CharactersResult(StrokeCollection strokes,int lang)
		{
			DAL.RecognizationCore _core = new DAL.RecognizationCore(lang);
			_core.Recognize(strokes);
			return _core.Alternates;
		}
	}
}
