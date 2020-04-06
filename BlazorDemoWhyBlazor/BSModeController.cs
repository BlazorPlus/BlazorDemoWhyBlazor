using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BlazorDemoWhyBlazor.Data;

namespace BlazorDemoWhyBlazor
{
	public class BSModeController : Controller
	{
		public class MessageBoardViewState
		{
			public string InputUserName { get; set; }
			public string InputTitle { get; set; }
			public string InputMessage { get; set; }
		}
		public class MessageBoardViewModel
		{
			public List<MyMsg> Msgs
			{
				get
				{
					return MyMsg.Msgs;
				}
			}

			public MessageBoardViewState State { get; set; } = new MessageBoardViewState();

			public string PostType { get; set; }
			public string PostResult { get; set; }
			public string PostResultMessage { get; set; }
		}

		[HttpGet]
		public IActionResult MessageBoard()
		{
			return View(new MessageBoardViewModel());
		}

		[HttpPost]
		public IActionResult MessageBoard(string post_action, string _state)
		{
			string[] actionarr = post_action.Split(':');

			MessageBoardViewModel vm = new MessageBoardViewModel();
			if (!string.IsNullOrEmpty(_state))
			{
				vm.State = System.Text.Json.JsonSerializer.Deserialize<MessageBoardViewState>(_state);
			}

			vm.PostType = actionarr[0];
			vm.State.InputUserName = Request.Form["inp_username"];
			vm.State.InputTitle = Request.Form["inp_title"];
			vm.State.InputMessage = Request.Form["inp_message"];

			if (vm.PostType == "PostMsg")
			{
				if (string.IsNullOrWhiteSpace(vm.State.InputUserName))
				{
					vm.PostResult = "Failed";
					vm.PostResultMessage = "Please type username";
					return View(vm);
				}
				if (string.IsNullOrWhiteSpace(vm.State.InputTitle))
				{
					vm.PostResult = "Failed";
					vm.PostResultMessage = "Please type title";
					return View(vm);
				}
				if (string.IsNullOrWhiteSpace(vm.State.InputMessage))
				{
					vm.PostResult = "Failed";
					vm.PostResultMessage = "Please type message";
					return View(vm);
				}

				MyMsg msg = MyMsg.AddMsg(vm.State.InputUserName, vm.State.InputTitle, vm.State.InputMessage);
				vm.PostResult = "Done";
				vm.PostResultMessage = "Message #" + msg.MsgId + " have been added";
				//Keep UserName //vm.State.InputUserName = "";
				vm.State.InputTitle = "";//clear Title
				vm.State.InputMessage = "";//clear Message
				return View(vm);
			}

			if (vm.PostType == "Delete" || vm.PostType == "Comment")
			{
				int id = int.Parse(actionarr[1]);
				var msg = MyMsg.Msgs.Where(v => v.MsgId == id).FirstOrDefault();
				if (msg == null)
				{
					vm.PostResult = "Failed";
					vm.PostResultMessage = "The message maybe deleted already";
					return View(vm);
				}
				if (vm.PostType == "Delete")
				{
					MyMsg.Msgs.Remove(msg);
					vm.PostResult = "Done";
					vm.PostResultMessage = "Message #" + msg.MsgId + " have been deleted";
					return View(vm);
				}
				if (vm.PostType == "Comment")
				{
					string comment = Request.Form["inp_comment_" + id];
					if (string.IsNullOrWhiteSpace(comment))
					{
						vm.PostResult = "Failed";
						vm.PostResultMessage = "Please type comment";
						return View(vm);
					}
					msg.Comments.Add(comment);
					vm.PostResult = "Done";
					vm.PostResultMessage = "Comment have been added";
					return View(vm);
				}
			}

			vm.PostResult = "Failed";
			vm.PostResultMessage = "Unknown action " + post_action;

			return View(vm);
		}

	}

}
