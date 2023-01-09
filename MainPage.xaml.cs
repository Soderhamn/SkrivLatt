using Android.Text.Style;

namespace SkrivLätt;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    CancellationTokenSource cts;

	public async void ReadText(string txt)
	{
        if(txt != null && txt.Length > 0) //txt får inte vara null eller tom sträng, då kraschar appen
        {
            CancelSpeech(); //Cancel på tidigare uppläst text

            SpeechOptions options = new SpeechOptions()
            {
                Pitch = 1.2f,   // 0.0 - 2.0
                Volume = 0.75f, // 0.0 - 1.0
            };

            cts = new CancellationTokenSource();
            await TextToSpeech.Default.SpeakAsync(txt, options, cancelToken: cts.Token);
        }
    }

    public void CancelSpeech()
    {
        if(cts?.IsCancellationRequested ?? true)
        {
            return;
        }
        cts.Cancel();
    }

    public void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		string oldText = e.OldTextValue;
		string newText = e.NewTextValue;
		string allText = inpTxt.Text;

        if(newText.Length > 0) //Annars kraschar programmet om newtext är tom
        {
            string lastLetter = newText[^1..];
            //await DisplayAlert("Alert", "Oldtext: " + oldText + " newtext: " + newText + " lastletter: " + lastLetter + " alltext: " + allText, "OK");
            ReadText(lastLetter);

            if (lastLetter == " ")
            {
                //Slpitta strängen på mellanslag
                string[] stringArr = allText.Split(' ');
                string lastWord = stringArr[stringArr.Length - 2]; //Sista ordet
                                                                   //DisplayAlert("Varning ", "Sista ordet: " + lastWord + " stingarr length: " + stringArr.Length, "Ok");
                ReadText(lastWord);
            }
        }
    }

	public void OnTextCompleted(object sender, EventArgs e)
	{
        ReadText(inpTxt.Text);
    }

    public void OnSettingsClicked(object sender, EventArgs e)
    {
        //Preferences.Default.Set("", "")
    }
}

