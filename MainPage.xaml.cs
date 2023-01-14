using System.Text.RegularExpressions;

namespace SkrivLätt;

public partial class MainPage : ContentPage
{

    public int CursorPosition { get; set; }

    

    public MainPage()
	{
		InitializeComponent();

        if (Preferences.Default.ContainsKey("fontFamily"))
        {
            string fontFamily = Preferences.Default.Get("fontFamily", "OpenDyslexic-Regular");
            inpTxt.FontFamily = fontFamily;
        }
        else
        {
            inpTxt.FontFamily = "OpenDyslexic-Regular";
        }

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

    //Gör en cancel på det som ligger i kö för att läsas upp / håller på att läsas upp
    public void CancelSpeech()
    {
        if(cts?.IsCancellationRequested ?? true)
        {
            return;
        }
        cts.Cancel();
    }

    //OnTextChanged körs när texten i Editorn ändrats
    public void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		string oldText = e.OldTextValue;
		string newText = e.NewTextValue;
		string allText = inpTxt.Text;

        if(newText.Length > 0) //Annars kraschar programmet om newtext är tom
        {
            string lastLetter = "";
            try
            {
                lastLetter = newText[inpTxt.CursorPosition - 1].ToString(); //Sista bokstaven, cursorposition fungerar inte vid inklistring
            }
            catch
            {
                lastLetter = newText[newText.Length - 1].ToString();
            }

            bool isSeparator = false;
            string regexPattern = @"[\?|\.|\!]";
            foreach(Match m in Regex.Matches(lastLetter, regexPattern))
            {
                isSeparator = true; 
            }

            if (lastLetter == " ")
            {
                //Slpitta strängen på mellanslag
                string[] stringArr = allText.Split(' ');
                string lastWord = stringArr[stringArr.Length - 2]; //Sista ordet

                ReadText(lastWord);
            }
            else if(isSeparator == true) //Det är någon av: .!?
            {
                string[] stringArr = Regex.Split(allText, regexPattern);
                string lastSentence = stringArr[stringArr.Length - 1];

                ReadText(lastSentence);
            }
            else //Om inte .!? eller mellanslag, läs upp senaste bokstaven
            {
                ReadText(lastLetter);
            }
        }
    }


    public async void OnReadAllClicked(object sender, EventArgs e)
    {
        ReadText(inpTxt.Text);
        btnReadAll.BackgroundColor = Colors.LimeGreen;
        btnReadAll.Text = "Läser upp text";

        await Task.Delay(3000);

        btnReadAll.BackgroundColor = Colors.Green;
        btnReadAll.Text = "Läs upp text";

    }

    public async void OnCopyTextClicked(object sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(inpTxt.Text);
        btnCopyText.Text = "Text kopierad till urklipp!";
        btnCopyText.TextColor = Colors.White;
        btnCopyText.Background = Colors.LimeGreen;

        await Task.Delay(1000);

        btnCopyText.Text = "Kopiera text";
        btnCopyText.TextColor = Colors.Black;
        btnCopyText.Background = Colors.White;
    }

    public async void OnPasteTextClicked(object sender, EventArgs e)
    {
        inpTxt.Text += await Clipboard.Default.GetTextAsync();

        btnPasteText.Text = "Text inklistrad!";
        btnPasteText.TextColor = Colors.White;
        btnPasteText.Background = Colors.LimeGreen;

        await Task.Delay(1000);

        btnPasteText.Text = "Klistra in text";
        btnPasteText.TextColor = Colors.Black;
        btnPasteText.Background = Colors.White;
    }

    


}

