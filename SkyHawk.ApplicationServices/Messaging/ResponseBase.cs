namespace SkyHawk.ApplicationServices.Messaging;

public abstract class ResponseBase
{
    public BusinessStatusCodeEnum StatusCode { get; set; } = BusinessStatusCodeEnum.None;
    public string? MessageText { get; set; }
};
