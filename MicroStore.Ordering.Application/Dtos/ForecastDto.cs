using Microsoft.ML.Data;

namespace MicroStore.Ordering.Application.Dtos
{
    public class ForecastDto
    {
        public float[] ForecastedValues { get; set; }

        public float[] ConfidenceLowerBound { get; set; }

        public float[] ConfidenceUpperBound { get; set; }

    }
}
