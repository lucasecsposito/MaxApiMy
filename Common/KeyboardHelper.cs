using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Common
{
	public static class KeyboardHelper
	{
		public static InlineKeyboardAttachment GetKeyboardAttachment(this IEnumerable<IEnumerable<string>> buttons, bool edit = false)
		{
			// Для нового сообщения клавиатура без кнопок должна возвращать null
			// При редактировании сообщения возможны два варианта:
			// если buttons == null, значит оставляем кнопки как есть
			// если buttons == пустой список, все кнопки будут удалены
			if (!edit && (buttons == null || buttons.Count() == 0 || (buttons.Count() == 1 && buttons.First().Count() == 0)))
				return null;
			if (edit && buttons == null)
				return null;
			if (edit && (buttons.Count() == 0 || (buttons.Count() == 1 && buttons.First().Count() == 0)))
				return new InlineKeyboardAttachment();
			var keyboardAttachment = new InlineKeyboardAttachment();
			var inlineButtons = keyboardAttachment.Payload.Buttons;
			var index = 0;
			foreach (var buttonsRowData in buttons)
			{
				var btnsRow = new List<Button>();
				foreach (var buttonData in buttonsRowData)
				{
					// В тексте кнопки можно передавать код кнопки
					// Пример: "Tuto::Обучение"
					var buttonCode = buttonData.Split("::").FirstOrDefault();
					var buttonText = buttonData.Split("::").LastOrDefault();
					// TODO :: (AK) поддержка других видов кнопок
					if (Uri.TryCreate(buttonCode, UriKind.Absolute, out Uri tmp))
						btnsRow.Add(new LinkButton(buttonText, buttonCode));
					else
						btnsRow.Add(new CallbackButton(buttonText, buttonCode));
					index++;
				}
				inlineButtons.Add(btnsRow);
			}
			return keyboardAttachment;
		}
	}
}
