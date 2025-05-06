using Blog.Admin.Core.Enum;

namespace Blog.Admin.Core.Entities;
/// <summary>
/// 可用状态
/// </summary>
public interface IAvailability
{
    /// <summary>
    /// 可用状态
    /// </summary>
    AvailabilityStatus Status { get; set; }
}