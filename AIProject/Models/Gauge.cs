using System;
using System.ComponentModel.DataAnnotations;

namespace AIProject.Models
{
    public class Gauge
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Serial No")]
        public string SerialNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Type { get; set; }

        [StringLength(100)]
        [Display(Name = "Sub Type")]
        public string? SubType { get; set; }

        [StringLength(100)]
        public string? Size { get; set; }

        [StringLength(100)]
        public string? Range { get; set; }

        [StringLength(200)]
        public string? Manufacturer { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Manufacture Date")]
        public DateTime? ManufactureDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Model No")]
        public string? ModelNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Equipment ID")]
        public string? EquipmentId { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [Display(Name = "Cal. Frequency")]
        public int? CalibrationFrequency { get; set; }

        [StringLength(100)]
        [Display(Name = "Cal. Frequency Type")]
        public string? CalibrationFrequencyType { get; set; }

        [StringLength(200)]
        [Display(Name = "Cal. Procedure")]
        public string? CalibrationProcedure { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Next Cal Date")]
        public DateTime? NextCalibrationDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Cal. Result")]
        public string? CalibrationResult { get; set; }

        [StringLength(100)]
        public string? Status { get; set; }

        [StringLength(150)]
        [Display(Name = "Calibration Cert #")]
        public string? CalibrationCertificateNumber { get; set; }

        [StringLength(150)]
        [Display(Name = "Received From")]
        public string? ReceivedFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        [StringLength(150)]
        [Display(Name = "Calibrated By")]
        public string? CalibratedBy { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Calibration Date")]
        public DateTime? CalibrationDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Location Calibrated")]
        public string? LocationCalibrated { get; set; }

        [StringLength(200)]
        [Display(Name = "Location of Records")]
        public string? LocationOfRecords { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Maintenance Due")]
        public DateTime? MaintenanceDueDate { get; set; }

        [StringLength(150)]
        public string? Owner { get; set; }

        [StringLength(150)]
        [Display(Name = "Gauge Type")]
        public string? GaugeType { get; set; }

        [StringLength(150)]
        [Display(Name = "UUT")]
        public string? Uut { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Comments { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Instructions { get; set; }

        [StringLength(500)]
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAtUtc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAtUtc { get; set; }
    }
}
