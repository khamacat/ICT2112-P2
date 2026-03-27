using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class DamageReportControl : iDamageReportCRUD, iDamageReportQuery
{
    private readonly IDamageReportMapper _damageReportMapper;

    public DamageReportControl(IDamageReportMapper damageReportMapper)
    {
        _damageReportMapper = damageReportMapper
            ?? throw new ArgumentNullException(nameof(damageReportMapper));
    }

    public bool SaveDamageReport(
        int returnItemId,
        string description,
        string severity,
        decimal? repairCost,
        string? imagePath)
    {
        var report = _damageReportMapper.FindByReturnItemId(returnItemId) ?? new Damagereport();
        report.SetReturnItemId(returnItemId);
        report.SetDescription(description);
        report.SetSeverity(severity);
        report.SetRepairCost(repairCost ?? 0m);
        report.SetReportDate(DateTime.UtcNow);
        report.SetImages(imagePath ?? string.Empty);

        if (!ValidateDamageReport(report)) return false;

        try
        {
            var existing = _damageReportMapper.FindByReturnItemId(returnItemId);

            if (existing is null)
            {
                _damageReportMapper.Insert(report);
            }
            else
            {
                existing.SetDescription(description);
                existing.SetSeverity(severity);
                existing.SetRepairCost(repairCost ?? 0m);
                existing.SetImages(imagePath ?? string.Empty);
                existing.SetReportDate(DateTime.UtcNow);
                _damageReportMapper.Update(existing);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AppendRepairNote(int returnItemId, string note)
    {
        try
        {
            var existing = _damageReportMapper.FindByReturnItemId(returnItemId);
            if (existing is null) return false;

            existing.SetDescription((existing.GetDescription() ?? string.Empty) + note);
            _damageReportMapper.Update(existing);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteDamageReport(int returnItemId)
    {
        try
        {
            var existing = _damageReportMapper.FindByReturnItemId(returnItemId);
            if (existing is null) return true;

            _damageReportMapper.Delete(existing);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Damagereport? GetDamageReportByReturnItem(int returnItemId)
    {
        return _damageReportMapper.FindByReturnItemId(returnItemId);
    }

    public List<Damagereport> GetDamageReportsByReturnRequest(int returnRequestId)
    {
        var all = _damageReportMapper.FindAll();
        if (all is null) return new List<Damagereport>();

        return all.ToList();
    }

    public byte[] DownloadDamageReport(int damageReportId)
    {
        var report = _damageReportMapper.FindById(damageReportId);
        if (report is null) return Array.Empty<byte>();

        var imageHtml = "";
        var imagePath = report.GetImages();

        if (!string.IsNullOrWhiteSpace(imagePath))
        {
            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                var ext = Path.GetExtension(fullPath).ToLower().TrimStart('.');
                var mimeType = ext == "jpg" || ext == "jpeg" ? "image/jpeg"
                    : ext == "png" ? "image/png"
                    : ext == "gif" ? "image/gif"
                    : "image/webp";

                var imgBytes = File.ReadAllBytes(fullPath);
                var b64 = Convert.ToBase64String(imgBytes);

                imageHtml =
                    $"<div class='row'><span class='label'>Damage Image:</span></div>" +
                    $"<img src='data:{mimeType};base64,{b64}' style='max-width:100%;margin-top:8px;border:1px solid #ddd;border-radius:4px;'/>";
            }
            else if (imagePath.StartsWith("http"))
            {
                imageHtml =
                    $"<div class='row'><span class='label'>Damage Image:</span></div>" +
                    $"<img src='{System.Net.WebUtility.HtmlEncode(imagePath)}' style='max-width:100%;margin-top:8px;'/>";
            }
        }

        var html = $@"<!DOCTYPE html>
<html><head><meta charset='utf-8'/>
<style>
  @page {{ size: A4; margin: 20mm; }}
  @media print {{ body {{ margin: 0; }} .no-print {{ display: none; }} }}
  body {{ font-family: Arial, sans-serif; font-size: 13px; color: #222; max-width: 794px; margin: 0 auto; padding: 40px; }}
  .header {{ border-bottom: 3px solid #c0392b; padding-bottom: 12px; margin-bottom: 20px; }}
  .header h1 {{ color: #c0392b; font-size: 22px; margin: 0 0 4px; }}
  .header .subtitle {{ color: #888; font-size: 11px; }}
  .section-title {{ font-size: 10px; font-weight: bold; text-transform: uppercase; letter-spacing: 0.08em; color: #999; margin: 16px 0 4px; }}
  .value {{ font-size: 14px; color: #222; }}
  .badge {{ display: inline-block; padding: 3px 12px; border-radius: 12px; background: #c0392b; color: white; font-size: 11px; font-weight: bold; }}
  .cost {{ font-size: 18px; font-weight: bold; color: #c0392b; }}
  hr {{ border: none; border-top: 1px solid #eee; margin: 16px 0; }}
  .footer {{ color: #bbb; font-size: 10px; margin-top: 40px; border-top: 1px solid #eee; padding-top: 10px; }}
  .print-btn {{ background:#c0392b;color:white;border:none;padding:8px 20px;border-radius:4px;cursor:pointer;font-size:13px;margin-bottom:20px; }}
  .breakdown {{ background:#fff5f5;border-left:3px solid #c0392b;padding:10px 14px;margin-top:8px;border-radius:0 4px 4px 0; }}
  .breakdown-title {{ margin:0 0 6px;font-weight:bold;color:#c0392b; }}
  .breakdown table {{ border-collapse:collapse;width:auto; }}
  .breakdown td {{ padding:2px 12px 2px 0;font-size:13px; }}
  .breakdown td.lbl {{ color:#888;width:130px; }}
  .breakdown tr.final td {{ font-weight:bold;color:#c0392b;border-top:1px solid #f5c6c6;padding-top:6px; }}
</style></head><body>
<button class='print-btn no-print' onclick='window.print()'>&#128438; Print / Save as PDF</button>
<div class='header'>
  <h1>&#9888; Damage Report</h1>
  <div class='subtitle'>Return Item #{report.GetReturnItemId()} &nbsp;&bull;&nbsp; Report #{damageReportId} &nbsp;&bull;&nbsp; Pro Rentals</div>
</div>
<div class='section-title'>Description</div>
<div class='value'>{RenderDescription(report.GetDescription() ?? "—")}</div>
<hr/>
<table width='100%'><tr>
  <td width='50%'>
    <div class='section-title'>Severity</div>
    <div><span class='badge'>{System.Net.WebUtility.HtmlEncode(report.GetSeverity() ?? "—")}</span></div>
  </td>
  <td width='50%'>
    <div class='section-title'>Repair Cost</div>
    <div class='cost'>${report.GetRepairCost()?.ToString("F2") ?? "0.00"}</div>
  </td>
</tr></table>
<hr/>
<div class='section-title'>Date Reported</div>
<div class='value'>{report.GetReportDate():dd MMM yyyy HH:mm} (UTC)</div>
{(string.IsNullOrWhiteSpace(imageHtml) ? "" : $"<hr/>{imageHtml}")}
<div class='footer'>Generated by Pro Rentals Return Processing System &nbsp;&bull;&nbsp; {DateTime.UtcNow:dd MMM yyyy HH:mm} UTC</div>
</body></html>";

        return System.Text.Encoding.UTF8.GetBytes(html);
    }

    private static string RenderDescription(string text)
    {
        if (!text.Contains("Price breakdown:"))
        {
            return System.Net.WebUtility.HtmlEncode(text).Replace("\n", "<br/>");
        }

        var parts = text.Split(new[] { "\n\nUnable to repair." }, 2, StringSplitOptions.None);
        var mainText = System.Net.WebUtility.HtmlEncode(parts[0].Trim()).Replace("\n", "<br/>");
        var breakdown = parts.Length > 1 ? parts[1] : "";

        var lines = breakdown.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var rows = new System.Text.StringBuilder();
        var finalRow = "";

        foreach (var line in lines)
        {
            if (line.StartsWith("Price breakdown")) continue;

            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0) continue;

            var label = line[..colonIdx].Trim();
            var value = line[(colonIdx + 1)..].Trim();
            var encoded = System.Net.WebUtility.HtmlEncode(value);

            if (label.StartsWith("Final"))
            {
                finalRow = $"<tr class='final'><td class='lbl'>{label}</td><td>{encoded}</td></tr>";
            }
            else
            {
                rows.Append($"<tr><td class='lbl'>{label}</td><td>{encoded}</td></tr>");
            }
        }

        return
            $"{mainText}<div class='breakdown'>" +
            $"<p class='breakdown-title'>&#9888; Unable to repair</p>" +
            $"<table>{rows}{finalRow}</table></div>";
    }

    private bool ValidateDamageReport(Damagereport damageReport)
    {
        if (damageReport is null) return false;
        if (string.IsNullOrWhiteSpace(damageReport.GetDescription())) return false;
        if (string.IsNullOrWhiteSpace(damageReport.GetSeverity())) return false;
        if (damageReport.GetRepairCost() is null || damageReport.GetRepairCost() < 0) return false;
        if (damageReport.GetReturnItemId() <= 0) return false;

        return true;
    }
}