using Blog.Admin.Core.Enum;

namespace Blog.Admin.Core.Shared;

public class AvailabilityDto : KeyDto
{
    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
}