using Microsoft.Maui.Graphics.Converters;
using System.Text.RegularExpressions;

namespace SkrivLätt;

public partial class MainPage : ContentPage
{
    public int CursorPosition { get; set; }

    public MainPage()
	{
		InitializeComponent();

    }

    CancellationTokenSource cts; //För att kunna avbryta uppläsning

    //Funktion för att läsa upp texten
	public async void ReadText(string txt)
	{
        if(txt != null && txt.Length > 0) //txt får inte vara null eller tom sträng, då kraschar appen
        {
            CancelSpeech(); //Cancel på tidigare uppläst text

            SpeechOptions options = new SpeechOptions() //Inställningar för uppläsningen
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
		//string oldText = e.OldTextValue;
		string newText = e.NewTextValue;
		string allText = inpTxt.Text;

        if(newText.Length > 0) //Kör bara om strängen har innehåll, annars kraschar programmet om newtext är tom
        {
            string lastLetter;
            try
            {
                //Hitta sista bokstaven, detta fungerar dock inte vid inklistring (ctrl+v)
                lastLetter = newText[inpTxt.CursorPosition - 1].ToString(); 
            }
            catch
            {
                //Hitta sista bokstaven om sättet ovan misslyckats, exempelvis vid inklistring av text
                lastLetter = newText[newText.Length - 1].ToString();
            }

            /* //Bygga vidare på i framtiden: Läs upp senaste meningen vid ./!/?
            bool isSeparator = false;
            string regexPattern = @"[\?|\.|\!]";
            foreach(Match m in Regex.Matches(lastLetter, regexPattern))
            {
                isSeparator = true; 
            }
            */

            if (lastLetter == " ") //Om användaren skrivit in mellanslag
            {
                //Slpitta strängen på mellanslag
                string[] stringArr = allText.Split(' ');
                string lastWord = stringArr[stringArr.Length - 2]; //Plocka ut sista ordet

                ReadText(lastWord);
            }
            //Utveckla i framtiden för att läsa ut senaste meningen
            /*else if(isSeparator == true) //Det är någon av: .!?
            {
                string[] stringArr = Regex.Split(allText, regexPattern);
                string lastSentence = stringArr[stringArr.Length - 1];

                ReadText(lastSentence);
            }*/
            else //Om inte .!? eller mellanslag, läs upp senaste bokstaven
            {
                ReadText(lastLetter);
            }
        }
    }

    //Om användaren klickat på "läs upp all text"
    public async void OnReadAllClicked(object sender, EventArgs e)
    {
        ReadText(inpTxt.Text);
        btnReadAll.BackgroundColor = Colors.LimeGreen;
        btnReadAll.Text = "Läser upp text";

        await Task.Delay(3000);

        btnReadAll.BackgroundColor = Colors.Green;
        btnReadAll.Text = "Läs upp text";

    }

    //Om användaren klickat på kopiera text
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

    //Klistra in text-knappen
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

    //När denna Sida (MainPage) visas så körs OnAppearing()
    //Läser in valda inställningar, eller standardinställningar om inget valts.
    protected override void OnAppearing()
    {
        if (Preferences.Default.ContainsKey("fontFamily") || Preferences.Default.ContainsKey("editorTextColor") || Preferences.Default.ContainsKey("editorBackgroundColor"))
        {
            string fontFamily = Preferences.Default.Get("fontFamily", "OpenDyslexic-Regular"); //Ladda in sparat typsnitt, om inget finns välj OpenDyslexic-Regular
            string strFontColor = Preferences.Default.Get("editorTextColor", "DarkBlue");
            string strBackgroundColor = Preferences.Default.Get("editorBackgroundColor", "PaleGoldenrod");

            //Hittade hur man konverterar string till färger här: https://stackoverflow.com/questions/72902119/net-maui-is-it-possible-to-convert-a-string-to-a-color-inside-a-binding 
            ColorTypeConverter converter = new ColorTypeConverter();
            Color fontColor = (Color)(converter.ConvertFromInvariantString(strFontColor));
            Color backgroundColor = (Color)(converter.ConvertFromInvariantString(strBackgroundColor));


            inpTxt.FontFamily = fontFamily;
            inpTxt.TextColor = fontColor;
            inpTxt.BackgroundColor = backgroundColor;

            //Felsökning:
            //await DisplayAlert("Font", "Font finns: " + fontFamily + " EditorTextColor: " + strFontColor + " Backgroundcolor: " + strBackgroundColor, "OK");
        }
        else
        {
            inpTxt.FontFamily = "OpenDyslexic-Regular";
            //await DisplayAlert("Font", "Font finns INTE: ", "OK");
        }
    }


}

