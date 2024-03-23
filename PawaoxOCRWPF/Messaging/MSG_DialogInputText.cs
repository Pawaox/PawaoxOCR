using PawaoxOCRWPF.GUI.Views.Windows.Options;
using PawaoxOCRWPF.Messaging;
using System;

public class MSG_DialogInputText : BaseMessage
{
	public WindowInputTextOptions Options { get; private set; }

	public Action<Response> OnClose { get; set; }

	public MSG_DialogInputText(WindowInputTextOptions options, Action<Response> onClose)
	{
		this.Options = options;

		this.OnClose = onClose;
	}

	public class Response
	{
		public bool DidContinue { get; set; }
		public string PrimaryText { get; set; } = "";
		public string SecondaryText { get; set; } = "";

		public Response() { }
		public Response(bool didCont, string primaryText, string secondaryText)
		{
			this.DidContinue = didCont;
			this.PrimaryText = primaryText;
			this.SecondaryText = secondaryText;
		}
	}
}