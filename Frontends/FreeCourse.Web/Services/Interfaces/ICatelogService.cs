using FreeCourse.Web.Models.Catelogs;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface ICatelogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();

        Task<List<CategoryViewModel>> GetAllCategoryAsync();

        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

        Task<CourseViewModel> GetByCourseId(string courseId);

        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);

        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);

        Task<bool> DeleteCourseAsync(string courseId);
    }
}
