using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Framework.Utils
{
    internal sealed class TokenStatistics
    {
        private static readonly TokenStatistics _instance = new TokenStatistics();

        private float tokensRequestingIdentification = 0.0f;
        private int successfullTokenIdentification = 0;
        private int failedTokenIdentification = 0;
        private float succesfullTokenIdentificationPercentage = 0.0f;

        private float identificationTime = 0.0f;
        private float totalIdentificationTime = 0.0f;
        private float avgIdentificationTime = 0.0f;
        private int identificationTokenTimeCounter = 0;

        private int expectedTokenClass = 0;
        private int totalClassComputedTokens = 0;
        private int successfullClassRecon = 0;

        public int TokenIdentificationSuccessRate
        {
            get
            {
                return (int)Math.Round(succesfullTokenIdentificationPercentage, 0, MidpointRounding.AwayFromZero);
            }
        }

        public float LastTokenIdentificationTime { get { return (float)Math.Round(identificationTime, 2, MidpointRounding.AwayFromZero); } }

        public float AvgTokenIdentificationTime
        {
            get
            {
                return (float)Math.Round(totalIdentificationTime / identificationTokenTimeCounter, 2, MidpointRounding.AwayFromZero);
            }
        }

        public int TotalTokens { get { return (int)tokensRequestingIdentification; } }
        public int FailedIdentificationTokens { get { return failedTokenIdentification; } }

        public static TokenStatistics Instance
        {
            get
            {
                return _instance;
            }
        }

        public int ExpectedTokenClass
        {
            get
            {
                return expectedTokenClass;
            }
            set
            {
                expectedTokenClass = value;
                totalClassComputedTokens = 0;
                successfullClassRecon = 0;

            }
        }

        public int TotalTokenClassRequest { get { return totalClassComputedTokens; } }
        public int SuccessfullTokenClassRecon { get { return successfullClassRecon; } }

        private TokenStatistics() { }

        internal void ResetMetrics()
        {
            tokensRequestingIdentification = 0.0f;
            successfullTokenIdentification = 0;
            failedTokenIdentification = 0;
            succesfullTokenIdentificationPercentage = 0.0f;

            identificationTime = 0.0f;
            totalIdentificationTime = 0.0f;
            avgIdentificationTime = 0.0f;
            identificationTokenTimeCounter = 0;


    }

        internal void TokenIdentification(bool succesfull)
        {
            tokensRequestingIdentification++;

            if (succesfull)
            {
                successfullTokenIdentification++;
                succesfullTokenIdentificationPercentage = (successfullTokenIdentification / tokensRequestingIdentification) * 100;
            }
            else
                failedTokenIdentification++;
        }

        internal void SetTokenIdentificationTime(float identificationTimeMs)
        {
            identificationTokenTimeCounter++;
            identificationTime = identificationTimeMs;
            totalIdentificationTime += identificationTime;
        }

        internal void TokenClassRecognition(int? tokenClass)
        {
            totalClassComputedTokens++;
            if (tokenClass == expectedTokenClass)
                successfullClassRecon++;
        }
        
    }
}
