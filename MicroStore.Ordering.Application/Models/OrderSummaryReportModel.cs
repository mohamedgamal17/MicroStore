﻿namespace MicroStore.Ordering.Application.Models
{
    public class OrderSummaryReportModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReportPeriod Period { get; set; }
        public string? Status { get; set; }
    }
}
