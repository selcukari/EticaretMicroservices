using AutoMapper;
using FreeCourse.Services.Catelog.Models;
using FreeCourse.Services.Catelog.Settings;
using FreeCourse.Shared.Dto;
using FreeCourse.Shared.Messages;
using Mass = MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace FreeCourse.Services.Catelog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, Mass.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;

            _publishEndpoint = publishEndpoint;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);

            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    if(!string.IsNullOrEmpty(course.CategoryId))
                    {
                        course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                    }
                    else
                    {
                        course.Category = new Category() { Name = "Kategori Bulunamadı" };
                    }
                }
            }
            else
            {
                courses = new List<Course>(); // yok ise boş bir liste döndür
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    if (!string.IsNullOrEmpty(course.CategoryId))
                    {
                        course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                    }
                    else
                    {
                        course.Category = new Category() { Name = "Kategori Bulunamadı" };
                    }
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }

            if (!string.IsNullOrEmpty(course.CategoryId))
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
            else
            {
                course.Category = new Category() { Name = "Kategori Bulunamadı" };
            }

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var existingCourse = await _courseCollection.Find(x => x.Id == courseUpdateDto.Id).FirstOrDefaultAsync();
            if (existingCourse == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            updateCourse.CreatedTime = existingCourse.CreatedTime;
            updateCourse.UpdateTime = DateTime.Now;

            // data update işlemi yapılır
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            // RabbitMQ Publish et, event at, eventi dinleyen servisler varsa onlar tetiklenir
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent { CourseId = updateCourse.Id, UpdatedName = courseUpdateDto.Name });

            return Response<NoContent>.Success(204);
        }
    }
}
