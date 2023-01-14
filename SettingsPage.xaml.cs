namespace SkrivLätt;

public partial class SettingsPage : ContentPage
{

    public SettingsPage()
	{
		InitializeComponent();
	}

	public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
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

}