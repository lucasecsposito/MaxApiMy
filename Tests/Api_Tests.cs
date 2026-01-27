using Common;
using MaxApiMy.Business;
using MaxApiMy.Common;
using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Tests
{
	public class Api_Tests
	{
		public async Task Test1()
		{
			var api = new Api("f9LHodD0cOK5Hfc3N_eq0Vbku46BLikMjuawboLuRFsaJ_tbaLkjRFt6s6znDFtbHv6C38c_430qcHDzHoFu");
			//UpdateList ul = api.Updates();
			//string json = File.ReadAllText("Assets/Messages_Keyboard.txt");
			//string json = File.ReadAllText("Assets/Messages_Photo.txt");
			//string json = File.ReadAllText("Assets/Answers.txt");
			string json = File.ReadAllText("Assets/Notification.txt");
			User bot = await api.Me();
			//var result = api.SendPhoto(28995974, "Text with buttons", "https://wallpapershome.com/images/pages/pic_h/26295.jpg", "callback_code::Captcha 3");
			//var result = api.SendTextAsync(28995974, "Text with buttons", "callback_code::Captcha 3");
			//var list = api.UpdatesAsync().Result;
			//var callbackId = (list.Updates[0] as MessageCallbackUpdate).Callback.CallbackId;
			//json = json.Replace("{CallbackId}", callbackId);
			//var result2 = await api.AnswersAsync(callbackId, json);
			//Message message = api.SendText(134734681, "New text2");
			//Message message = api.SendPhoto(134734681, "Photo1", "https://wallpapershome.com/images/pages/pic_h/26295.jpg");
			//Message message = api.SendPhoto(134734681, "Photo1", "https://wallpapershome.com/images/pages/pic_h/26295.jpg", "https://ngs.ru::NGS;callback_code::Callback Button");
			//string result = api.SendPhotos(134734681, "Photo1", new string[] { "https://wallpapershome.com/images/pages/pic_h/26295.jpg", "https://academy.allaboutbirds.org/wp-content/uploads/2015/07/Painted_Bunting_male_Birdhsare_Tim_Hopwood.jpg" }.ToList());
			//Message message = api.SendPhotos(134734681, "Photo1", new string[] { "https://wallpapershome.com/images/pages/pic_h/26295.jpg", "https://academy.allaboutbirds.org/wp-content/uploads/2015/07/Painted_Bunting_male_Birdhsare_Tim_Hopwood.jpg" }.ToList(), "https://ngs.ru::NGS;callback_code::Callback Button");
			//string result = api.SendVideo(134734681, "Video1", "https://file-examples.com/wp-content/storage/2017/04/file_example_MP4_480_1_5MG.mp4");
			//string result = api.SendText(134734681, "Text1");
			//string result = api.EditText("mid.000000000fec6cec019b985747fe2632", "Edited text5", null);
			//string result = api.EditText("mid.000000000fec6cec019b985747fe2632", "Edited text6", "");
			//string result = api.EditText("mid.000000000fec6cec019b985747fe2632", "Edited text7", "https://ngs.ru::NGS;callback_code::Callback Button");
			//string result = api.EditPhoto("mid.000000000fec6cec019b985747fe2632", "Edited photo1", null, null);
			//string result = api.EditPhoto("mid.000000000fec6cec019b985747fe2632", "Edited photo4", "https://www.birds.cornell.edu/home/wp-content/uploads/2023/09/334289821-Baltimore_Oriole-Matthew_Plante.jpg", null);
			//string result = api.EditPhoto("mid.000000000fec6cec019b985747fe2632", "Edited photo2", "https://tx.audubon.org/sites/default/files/styles/bean_wysiwyg_full_width/public/cbcpressroom_tuftedtitmouse-judyhowle.jpg", "");
			//SimpleQueryResult result = api.EditPhoto("mid.000000000fec6cec019b985747fe2632", "Edited photo3", "https://wallpapershome.com/images/pages/pic_h/26295.jpg", "https://ngs.ru::NGS;callback_code::Callback Button");
			//SimpleQueryResult result = api.MessageDelete("mid.000000000fec6cec019b985747fe2632");
			//mid.000000000fec6cec019b985747fe2632
			//Message message = api.MessageGet("mid.000000000fec6cec019b985747fe2632");
			//SimpleQueryResult result = api.AnswerText("f9LHodD0cOJHgP31His3yQEdyMfrWqyPL1XtNAEmhaVVimB7aLGlv4gaFZKEgqA1THGoXHdO-xvzvzqKm-fa4t48P5WG7gD6CQ-6urhH-96Bwe597Smv", "Callback text5", "https://ngs.ru::NGS;callback_code::Callback Button", "notification text2");
			//SimpleQueryResult result = api.AnswerNotification("f9LHodD0cOJHgP31His3yQEdyMfrWqyPL1XtNAEmhaVVimB7aLGlv4gaFZKEgqA1THGoXHdO-xvzvzqKm-fa4t48P5WG7gD6CQ-6urhH-96Bwe597Smv", "notification text");
			//SimpleQueryResult result = api.AnswerPhoto("f9LHodD0cOJHgP31His3yQEdyMfrWqyPL1XtNAEmhaVVimB7aLGlv4gaFZKEgqA1THGoXHdO-xvzvzqKm-fa4t48P5WG7gD6CQ-6urhH-96Bwe597Smv", "Callback text5", "https://www.mercedes-benz.com.my/content/dam/malaysia/passengercars/campaign/cny-2026/cny26-GLC-350e.jpg", "https://ngs.ru::NGS;callback_code::Callback Button", "notification photo");
			//SimpleQueryResult result = api.AnswerPhotos("f9LHodD0cOJHgP31His3yQEdyMfrWqyPL1XtNAEmhaVVimB7aLGlv4gaFZKEgqA1THGoXHdO-xvzvzqKm-fa4t48P5WG7gD6CQ-6urhH-96Bwe597Smv", "Callback text5", new string[] { "https://www.mercedes-benz.com.my/content/dam/malaysia/passengercars/campaign/cny-2026/cny26-GLC-350e.jpg", "https://yumove.co.uk/cdn/shop/articles/custom_resized_1cc2e5e7-0533-4a07-8405-ea0fb000cb86.jpg?v=1763561868&width=1240" }.ToList(), "https://ngs.ru::NGS;callback_code::Callback Button", "notification photo");
			//Chat chat = api.Сhats_Get(-69681306954329);
			//Console.WriteLine(result2.Message + result2.Success);
			//var result = await api.SendTextByUserIdAsync(134734681, "Message by userId2", null);
			//var result = await api.SendTextAsync(134734681, "Message in bot by userId6");
			var result = await api.SendPhotoAsync(134734681, "Message in bot by userId6", "https://icdn.lenta.ru/images/2026/01/22/13/20260122131244848/owl_detail_620_d57516041d5b93425225c48f47a62b71.jpg");
			Console.WriteLine(result);
			//result = await api.SendTextAsync(-70406918203993, "Message in chat by userId6");
			result = await api.SendPhotoAsync(-70406918203993, "Message in chat by userId6", "https://icdn.lenta.ru/images/2026/01/22/13/20260122131244848/owl_detail_620_d57516041d5b93425225c48f47a62b71.jpg");
			Console.WriteLine(result);
		}
	}
}
