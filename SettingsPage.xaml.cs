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

			lblFontStatus.Text = "Teckensnitt uppdaterat till: " + fontToSave;
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

            lblFontStatus.Text = "Teckensnittsfärg uppdaterad till: " + fontColorToSave;
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

            lblFontStatus.Text = "Bakgrundsfärg uppdaterad till: " + backgroundColorToSave;
        }
    }

    public bool OnResetSettings()
	{
        //Ställ in standardinställningarna
        Preferences.Default.Set("fontFamily", "OpenDyslexic-Regular"); //Typsnitt
        Preferences.Default.Set("editorTextColor", "DarkBlue"); //Teckenfärg
        Preferences.Default.Set("editorBackgroundColor", "PaleGoldenrod"); //Bakgrundsfärg

        lblResetStatus.Text = "Inställningarna återställdes til standardinställningar";

		return true;
    }

}