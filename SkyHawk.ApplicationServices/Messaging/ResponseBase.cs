namespace SkyHawk.ApplicationServices.Messaging;

public abstract class ResponseBase
{
    public BusinessStatusCodeEnum StatusCode = BusinessStatusCodeEnum.None;
    public string? MessageText { get; set; }
};
