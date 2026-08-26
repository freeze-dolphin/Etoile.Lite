namespace Etoile.Lite.Parser.Utility;

public static class Easing
    {
        private const double Pi = Math.PI;

        private static readonly Dictionary<string, Func<double, double, double, double>> StringMapping = new()
        {
            { "linear", Linear },
            { "l", Linear },
            { "inconstant", InConst },
            { "inconst", InConst },
            { "cnsti", InConst },
            { "outconstant", OutConst },
            { "outconst", OutConst },
            { "cnsto", OutConst },
            { "inoutconstant", InOutConst },
            { "inoutconst", InOutConst },
            { "cnstb", InOutConst },
            { "insine", InSine },
            { "si", InSine },
            { "outsine", OutSine },
            { "so", OutSine },
            { "inoutsine", InOutSine },
            { "b", InOutSine },
            { "inquadratic", InQuad },
            { "inquad", InQuad },
            { "2i", InQuad },
            { "outquadratic", OutQuad },
            { "outquad", OutQuad },
            { "2o", OutQuad },
            { "inoutquadratic", InOutQuad },
            { "inoutquad", InOutQuad },
            { "2b", InOutQuad },
            { "incubic", InCubic },
            { "3i", InCubic },
            { "outcubic", OutCubic },
            { "outcube", OutCubic },
            { "3o", OutCubic },
            { "inoutcubic", InOutCubic },
            { "inoutcube", InOutCubic },
            { "3b", InOutCubic },
            { "inquartic", InQuart },
            { "inquart", InQuart },
            { "4i", InQuart },
            { "outquartic", OutQuart },
            { "outquart", OutQuart },
            { "4o", OutQuart },
            { "inoutquartic", InOutQuart },
            { "inoutquart", InOutQuart },
            { "4b", InOutQuart },
            { "inquintic", InQuint },
            { "inquint", InQuint },
            { "5i", InQuint },
            { "outquintic", OutQuint },
            { "outquint", OutQuint },
            { "5o", OutQuint },
            { "inoutquintic", InOutQuint },
            { "inoutquint", InOutQuint },
            { "5b", InOutQuint },
            { "inexponential", InExpo },
            { "inexpo", InExpo },
            { "exi", InExpo },
            { "outexponential", OutExpo },
            { "outexpo", OutExpo },
            { "exo", OutExpo },
            { "inoutexponential", InOutExpo },
            { "inoutexpo", InOutExpo },
            { "exb", InOutExpo },
            { "incircle", InCirc },
            { "incirc", InCirc },
            { "ci", InCirc },
            { "outcircle", OutCirc },
            { "outcirc", OutCirc },
            { "co", OutCirc },
            { "inoutcircle", InOutCirc },
            { "inoutcirc", InOutCirc },
            { "cb", InOutCirc },
            { "inback", InBack },
            { "bki", InBack },
            { "outback", OutBack },
            { "bko", OutBack },
            { "inoutback", InOutBack },
            { "bkb", InOutBack },
            { "inelastic", InElastic },
            { "eli", InElastic },
            { "outelastic", OutElastic },
            { "elo", OutElastic },
            { "inoutelastic", InOutElastic },
            { "elb", InOutElastic },
            { "inbounce", InBounce },
            { "bni", InBounce },
            { "outbounce", OutBounce },
            { "bno", OutBounce },
            { "inoutbounce", InOutBounce },
            { "bnb", OutBounce },
        };

        public static double Linear(double start, double end, double x)
        {
            return start + ((end - start) * x);
        }

        public static double InConst(double start, double end, double x)
        {
            return start;
        }

        public static double OutConst(double start, double end, double x)
        {
            return end;
        }

        public static double InOutConst(double start, double end, double x)
        {
            return (x >= 0.5) ? end : start;
        }

        public static double InSine(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Cos(x * Pi / 2)));
        }

        public static double OutSine(double start, double end, double x)
        {
            return start + ((end - start) * Sin(x * Pi / 2));
        }

        public static double InOutSine(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Cos(x * Pi)) / 2);
        }

        public static double InQuad(double start, double end, double x)
        {
            return start + ((end - start) * x * x);
        }

        public static double OutQuad(double start, double end, double x)
        {
            return start + ((end - start) * (1 - ((1 - x) * (1 - x))));
        }

        public static double InOutQuad(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? (2 * x * x) : (1 - ((2 - (2 * x)) * (2 - (2 * x)) / 2))));
        }

        public static double InCubic(double start, double end, double x)
        {
            return start + ((end - start) * (x * x * x));
        }

        public static double OutCubic(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Pow(1 - x, 3)));
        }

        public static double InOutCubic(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? 4 * x * x * x : 1 - (Pow((-2 * x) + 2, 3) / 2)));
        }

        public static double InQuart(double start, double end, double x)
        {
            return start + ((end - start) * (x * x * x * x));
        }

        public static double OutQuart(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Pow(1 - x, 4)));
        }

        public static double InOutQuart(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? 8 * x * x * x * x : 1 - (Pow((-2 * x) + 2, 4) / 2)));
        }

        public static double InQuint(double start, double end, double x)
        {
            return start + ((end - start) * (x * x * x * x * x));
        }

        public static double OutQuint(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Pow(1 - x, 5)));
        }

        public static double InOutQuint(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? 16 * x * x * x * x * x : 1 - (Pow((-2 * x) + 2, 5) / 2)));
        }

        public static double InExpo(double start, double end, double x)
        {
            return start + ((end - start) * (x == 0 ? 0 : Pow(2, (10 * x) - 10)));
        }

        public static double OutExpo(double start, double end, double x)
        {
            return start + ((end - start) * (x == 1 ? 1 : 1 - Pow(2, -10 * x)));
        }

        public static double InOutExpo(double start, double end, double x)
        {
            return start + ((end - start) * (x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Pow(2, (20 * x) - 10) / 2 : (2 - Pow(2, (-20 * x) + 10)) / 2));
        }

        public static double InCirc(double start, double end, double x)
        {
            return start + ((end - start) * (1 - Sqrt(1 - Pow(x, 2))));
        }

        public static double OutCirc(double start, double end, double x)
        {
            return start + ((end - start) * Sqrt(1 - Pow(x - 1, 2)));
        }

        public static double InOutCirc(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? (1 - Sqrt(1 - Pow(2 * x, 2))) / 2 : (Sqrt(1 - Pow((-2 * x) + 2, 2)) + 1) / 2));
        }

        public static double InBack(double start, double end, double x)
        {
            const double c1 = 1.70158;
            const double c3 = c1 + 1;

            return start + ((end - start) * ((c3 * x * x * x) - (c1 * x * x)));
        }

        public static double OutBack(double start, double end, double x)
        {
            const double c1 = 1.70158;
            const double c3 = c1 + 1;

            return start + ((end - start) * (1 + (c3 * Pow(x - 1, 3)) + (c1 * Pow(x - 1, 2))));
        }

        public static double InOutBack(double start, double end, double x)
        {
            const double c1 = 1.70158;
            const double c2 = c1 * 1.525;

            return start + ((end - start) * (x < 0.5 ? Pow(2 * x, 2) * (((c2 + 1) * 2 * x) - c2) / 2 : ((Pow((2 * x) - 2, 2) * (((c2 + 1) * ((x * 2) - 2)) + c2)) + 2) / 2));
        }

        public static double InElastic(double start, double end, double x)
        {
            const double c4 = (2 * Pi) / 3;

            return start + ((end - start) * (x == 0 ? 0 : x == 1 ? 1 : -Pow(2, (10 * x) - 10) * Sin(((x * 10) - 10.75) * c4)));
        }

        public static double OutElastic(double start, double end, double x)
        {
            const double c4 = (2 * Pi) / 3;

            return start + ((end - start) * (x == 0 ? 0 : x == 1 ? 1 : (Pow(2, -10 * x) * Sin(((x * 10) - 0.75) * c4)) + 1));
        }

        public static double InOutElastic(double start, double end, double x)
        {
            const double c5 = (2 * Pi) / 4.5;

            return start + ((end - start) * (x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? -(Pow(2, (20 * x) - 10) * Sin(((20 * x) - 11.125) * c5)) / 2 : (Pow(2, (-20 * x) + 10) * Sin(((20 * x) - 11.125) * c5) / 2) + 1));
        }

        public static double InBounce(double start, double end, double x)
        {
            return end - OutBounce(start, end, 1 - x);
        }

        public static double OutBounce(double start, double end, double x)
        {
            const double n1 = 7.5625;
            const double d1 = 2.75;

            return start + ((end - start) * (x < 1 / d1 ? 1 * x * x : x < 2 / d1 ? (1 * (x -= 1.5 / d1) * x) + 0.75 : x < 2.5 / d1 ? (1 * (x -= 2.25 / d1) * x) + 0.9375 : (n1 * (x -= 2.625 / d1) * x) + 0.984375));
        }

        public static double InOutBounce(double start, double end, double x)
        {
            return start + ((end - start) * (x < 0.5 ? (1 - OutBounce(0, 1, 1 - (2 * x))) / 2 : (1 + OutBounce(0, 1, (2 * x) - 1)) / 2));
        }

        public static Func<double, double, double, double> FromString(string s)
        {
            if (string.IsNullOrEmpty(s) || !StringMapping.ContainsKey(s))
            {
                return Linear;
            }

            return StringMapping[s];
        }

        private static double Cos(double x)
        {
            return Math.Cos(x);
        }

        private static double Sin(double x)
        {
            return Math.Sin(x);
        }

        private static double Pow(double x, double y)
        {
            return Math.Pow(x, y);
        }

        private static double Sqrt(double x)
        {
            return Math.Sqrt(x);
        }
    }