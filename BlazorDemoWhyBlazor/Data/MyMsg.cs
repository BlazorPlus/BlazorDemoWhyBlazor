using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDemoWhyBlazor.Data
{
	public class MyMsg
	{
		public int MsgId { get; set; }
		public DateTime PostTime { get; set; }
		public string UserName { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }

		public List<string> Comments { get; } = new List<string>();


		static public List<MyMsg> Msgs = new List<MyMsg>();

		static public int NextMsgId = 1;

		static public MyMsg AddMsg(string userame,string title, string message)
		{
			MyMsg msg = new MyMsg();
			msg.MsgId = NextMsgId++;
			msg.PostTime = DateTime.Now;
			msg.UserName = userame;
			msg.Title = title;
			msg.Message = message;
			Msgs.Add(msg);
			return msg;
		}
	}

}
