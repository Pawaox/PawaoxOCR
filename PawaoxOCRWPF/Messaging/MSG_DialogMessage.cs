using PawaoxOCRWPF.GUI.Views.Windows.Options;
using PawaoxOCRWPF.Messaging;
using System;

public class MSG_DialogMessage : BaseMessage
{
	public WindowMessageOptions Options { get; private set; }

	public Action OnClose { get; set; }

	public MSG_DialogMessage(WindowMessageOptions options, Action onClose)
	{
		this.Options = options;

		this.OnClose = onClose;
	}
}