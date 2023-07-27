﻿namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentSearchModel : BasePagedListModel
    {
        public string? OrderNumber { get; set; }
        public string? TrackingNumber { get; set; }
        public int? Status { get; set; }
        public string? Country { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}