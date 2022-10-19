namespace VRKeys.Layouts
{

    /// <summary>
    /// Korean language keyboard.
    /// </summary>
    public class Korean : Layout
    {
        public Korean()
        {
            placeholderMessage = "Ű�� ���� Ÿ������ �����ϼ���";

            spaceButtonLabel = "SPACE";

            enterButtonLabel = "ENTER";

            cancelButtonLabel = "CANCEL";

            shiftButtonLabel = "SHIFT";

            backspaceButtonLabel = "BACKSPACE";

            clearButtonLabel = "CLEAR";

            row1Keys = new string[] { "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=" };

            row1Shift = new string[] { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" };

            row2Keys = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "[", "]", "\\" };

            row2Shift = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "{", "}", "|" };

            row3Keys = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", ";", "'" };

            row3Shift = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", ":", "\"" };

            row4Keys = new string[] { "��", "��", "��", "��", "��", "��", "��", ",", ".", "?" };

            row4Shift = new string[] { "��", "��", "��", "��", "��", "��", "��", "<", ">", "/" };

            row1Offset = 0.16f;

            row2Offset = 0.08f;

            row3Offset = 0f;

            row4Offset = -0.08f;
        }
    }
}
