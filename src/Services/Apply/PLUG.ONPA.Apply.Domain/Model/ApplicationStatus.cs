using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Apply.Domain.Model;

public  sealed class ApplicationStatus : Enumeration
{
    public static ApplicationStatus Received = new(0);
    public static ApplicationStatus Valid = new(1);
    public static ApplicationStatus Invalid = new(2);
    public static ApplicationStatus InRecommendation = new(3);
    public static ApplicationStatus AwaitsDecision = new(4);
    public static ApplicationStatus Approved = new(5);
    public static ApplicationStatus Rejected = new(6);
    public static ApplicationStatus RejectionAppealed = new(7);
    public static ApplicationStatus AppealApproved = new(8);
    public static ApplicationStatus AppealDismissed = new(9);
    public static ApplicationStatus AppealRejected = new(10);
    public static ApplicationStatus Cancelled = new(11);
      
    [JsonConstructor]
    public ApplicationStatus(int value, [CallerMemberName] string displayName = "") : base(value, displayName)
    {
    }
}