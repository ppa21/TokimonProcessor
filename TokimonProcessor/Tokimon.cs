using System;
using System.Text;

namespace TokimonProcessor {
    public class Tokimon {
        private string tokiName;
        private string tokiId;
        private double tokiScore;
        private string tokiComment;
        private string teamComment;
        private string fromToki;
        private int Length = 2;

        public Tokimon(string tokiName, string tokiId, double tokiScore, string tokiComment, string teamComment) {
            this.tokiName = tokiName;
            this.tokiId = tokiId;
            this.tokiScore = tokiScore;
            this.tokiComment = tokiComment;
            this.teamComment = teamComment;
        }

        public string TokiName {
            get => tokiName;
        }

        public string TokiId {
            get => tokiId;
        }

        public double TokiScore {
            get => tokiScore;
        }

        public string TokiComment {
            get => tokiComment;
        }

        public string TeamComment {
            get => teamComment;
            set => teamComment = value;
        }

        public string FromToki {
            get => fromToki;
            set => fromToki = value;
        }

        private int ExtractValue(string str) {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < str.Length; i++) {
                if (builder.Length < Length && Char.IsDigit(str[i])) {
                    builder.Append(str[i]);
                }
            }

            return Int32.Parse(builder.ToString());
        }
        
        public int IntegerTokiId() {
            string str = TokiId;
            return ExtractValue(str);
        }

        public int ReceivedFrom() {
            string str = TokiComment;
            return ExtractValue(str);
        }

        public int TokimonTeamId() {
            string tokiId = IntegerTokiId().ToString();
            string teamTokiId = tokiId[0].ToString();

            return Int32.Parse(teamTokiId);
        }

        public string UniqueTokiId() {
            string str = TokiId.Trim();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i <= Length; i++) {
                builder.Append(str[i]);
            }

            return builder.ToString();
        }

        public override string ToString() {
            return TokiName + ", " + TokiId + ", " + TokiScore + ", " + TokiComment + ", " + teamComment;
        }
    }
}