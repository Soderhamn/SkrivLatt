namespace SkrivLätt;

public partial class SettingsPage : ContentPage
{

    public SettingsPage()
	{
		InitializeComponent();
	}

	public void OnFontPickerSelectedIndexChanged(object sender, EventArgs e)
	{
		var picker = (Picker)sender;
		int selectedIndex = picker.SelectedIndex;

		if(selectedIndex != -1)
		{
			string fontToSave = picker.Items[selectedIndex]; //Sträng valt teckensnitt

			Preferences.Default.Set("fontFamily", fontToSave); //Spara inställningen 

			lblUpdateStatus.Text = "Teckensnitt uppdaterat till: " + fontToSave;
        }
	}

	public void OnFontColorPickerSelectedIndexChanged(object sender, EventArgs e)
	{
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            string fontColorToSave = picker.Items[selectedIndex]; //Sträng vald teckenfärg

            Preferences.Default.Set("editorTextColor", fontColorToSave); //Spara inställningen 

            lblUpdateStatus.Text = "Teckensnittsfärg uppdaterad till: " + fontColorToSave;
        }
    }

    public void OnBackgroundColorPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            string backgroundColorToSave = picker.Items[selectedIndex]; //Sträng vald teckenfärg

            Preferences.Default.Set("editorBackgroundColor", backgroundColorToSave); //Spara inställningen 

            lblUpdateStatus.Text = "Bakgrundsfärg uppdaterad till: " + backgroundColorToSave;
        }
    }

    public void OnResetSettings(object sender, EventArgs e)
    {
        //Ställ in standardinställningarna
        Preferences.Default.Set("fontFamily", "OpenDyslexic-Regular"); //Typsnitt
        Preferences.Default.Set("editorTextColor", "DarkBlue"); //Teckenfärg
        Preferences.Default.Set("editorBackgroundColor", "PaleGoldenrod"); //Bakgrundsfärg

        lblUpdateStatus.Text = "Inställningarna återställdes til standardinställningar";
    }

}