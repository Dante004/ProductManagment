using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExtension
    {
        public static async Task<PaginationResult<TDto>> ToPagedListAsync<TEntity, TDto>(this IQueryable<TEntity> source,
            Paging paging,
            IMapper _mapper,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            var count = await source.CountAsync(cancellationToken);
            var items = source.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize);

            var itemsDto = _mapper.ProjectTo<TDto>(items);

            return new PaginationResult<TDto>
            {
                Items = itemsDto.AsEnumerable(),
                Page = paging.PageNumber,
                Size = paging.PageSize,
                Count = count,
                Success = true
            };
        }
    }
}
