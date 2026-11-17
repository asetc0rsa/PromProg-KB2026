using Hospital.BusinessObjects;
using Hospital.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new HospitalService(Path.Combine(builder.Environment.ContentRootPath, "Data")));
var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (HospitalException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

app.UseDefaultFiles();
app.UseStaticFiles();

var api = app.MapGroup("/api");
api.MapGet("/departments", (HospitalService s) => s.GetDepartments());
api.MapGet("/staff", (HospitalService s) => s.GetStaff());
api.MapGet("/records", (HospitalService s) => s.GetRecords());
api.MapPost("/admissions", (AdmissionRequest r, HospitalService s) => s.Admit(r));
api.MapPost("/records/{id:guid}/refer", (Guid id, ReferRequest r, HospitalService s) => s.Refer(id, r));
api.MapPost("/records/{id:guid}/prescriptions", (Guid id, PrescriptionRequest r, HospitalService s) => s.AddPrescription(id, r));
api.MapPost("/records/{id:guid}/prescriptions/{prescriptionId:guid}/perform", (Guid id, Guid prescriptionId, PerformRequest r, HospitalService s) => s.Perform(id, prescriptionId, r));
api.MapPost("/records/{id:guid}/discharge", (Guid id, HospitalService s) => s.Discharge(id));
api.MapGet("/records/{id:guid}/epicrisis", (Guid id, HospitalService s) => s.GetSummary(id));

app.Run();
