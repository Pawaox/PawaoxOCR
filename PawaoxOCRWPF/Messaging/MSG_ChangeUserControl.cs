using PawaoxOCRWPF.GUI.GUIModels;
using PawaoxOCRWPF.Messaging;

public class MSG_ChangeUserControl : BaseMessage
{
	public UserControlType Type { get; private set; }

	public MSG_ChangeUserControl(UserControlType type)
	{
		this.Type = type;
	}
}