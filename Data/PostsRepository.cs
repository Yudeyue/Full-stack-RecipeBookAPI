using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace RecipeBookAPI.Data
{
    [EnableCors("CORSPolicy")]
    public class PostsRepository
    {
        [EnableCors("CORSPolicy")]
        internal async static Task<List<Post>> GetPostsAsync()
        {
            using (var db = new AppDBContext())
            {
                return await db.Posts.ToListAsync();
            }
        }

        [EnableCors("CORSPolicy")]
        internal async static Task<Post> GetPostByIdAsync(int postId)
        {
            using (var db = new AppDBContext())
            {
                return await db.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
            }
        }

        [EnableCors("CORSPolicy")]
        internal async static Task<bool> CreatePostAsync(Post createPost)
        {
            using(var db = new AppDBContext())
            {
                try
                {
                    await db.Posts.AddAsync(createPost);
                    return await db.SaveChangesAsync() >= 1;

                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }

        [EnableCors("CORSPolicy")]
        internal async static Task<bool> UpdatePostAsync(Post updatePost)
        {
            using (var db = new AppDBContext())
            {
                try
                {
                    db.Posts.Update(updatePost);
                    return await db.SaveChangesAsync() >= 1;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }

        [EnableCors("CORSPolicy")]
        internal async static Task<bool> DeletePostAsync(int postId)
        {
            using (var db = new AppDBContext())
            {
                try
                {
                    Post postToDelete = await GetPostByIdAsync(postId);
                    db.Posts.Remove(postToDelete);
                    return await db.SaveChangesAsync() >= 1;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }
    }
}
