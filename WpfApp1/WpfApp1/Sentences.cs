using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Sentences
    {
        private List<string> _coldSentences;
        public List<string> ColdSentences
        {
            get { return _coldSentences; }
            set { _coldSentences = value; }
        }
        private List<string> _okSentences;
        public List<string> OkSentences
        {
            get { return _okSentences; }
            set { _okSentences = value; }
        }
        private List<string> _hotSentences;
        public List<string> HotSentences
        {
            get { return _hotSentences; }
            set { _hotSentences = value; }
        }

        public int _sentenceCount;
        private int SentenceCount
        {
            get { return _sentenceCount; }
            set
            {
                if (_sentenceCount != value)
                    _sentenceCount = value;
            }
        }
        public Sentences()
        {
            HotSentences = new List<string>();
            HotSentences.Add("My god, it's freaking hot in there!");
            HotSentences.Add("Don't forget to drink iced water to cool you down.");
            HotSentences.Add("I don't wanna move, I'm already sweating too much for that.");
            HotSentences.Add("Can you turn on the AC please ?");
            HotSentences.Add("Never go to dallas in July, temperature can achieve 96°C there! it's unbelievibly hot.");

            OkSentences = new List<string>();
            OkSentences.Add("I feel fine today, how are you ?");
            OkSentences.Add("I could go for some tea right about now...");
            OkSentences.Add("I'm hungry.");
            OkSentences.Add("it's not raining today, right ?");
            OkSentences.Add("Did you know the human body can survive an average of 3 days without water ?");

            ColdSentences = new List<string>();
            ColdSentences.Add("Damn, it's really cold here...");
            ColdSentences.Add("Winter is not coming, it's already here!");
            ColdSentences.Add("Why am I so cold? I'm literally wearing 1t of clothes.");
            ColdSentences.Add("Don't stay out too long, you'll get frotbites.");
            ColdSentences.Add("Aren't we in Dudinka ? It's the coldest town in the world with an average of -33°C in January\nHow can people live there?");
            SentenceCount = 0;
        }

        public string GetSentence(float temperature, DateTime timer_method)
        {
            Random rnd = new Random();
            string toReturn = "";
            if (timer_method >= DateTime.Now)
                SentenceCount = rnd.Next(0, HotSentences.Count);
            if (temperature > 30.0f)
                toReturn = HotSentences[SentenceCount];
            else if (temperature < 5.0f)
                toReturn = ColdSentences[SentenceCount];
            else
                toReturn = OkSentences[SentenceCount];
            return toReturn;
        }
    }
}
