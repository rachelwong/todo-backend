using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.models;

namespace TodoBackend.controller;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoController(TodoContext context)
    {
        _context = context;
    }
    
    // GET (all items): api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }
    
    // POST (create one): api/todo
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        TodoItem newTodo = new TodoItem { Id = Guid.NewGuid().ToString(), Description = todoItem.Description, Done = todoItem.Done };
        _context.TodoItems.Add(newTodo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTodoItems), new { id = newTodo.Id }, newTodo);
    }
    
    // PUT (edit one): api/todo
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> EditTodoItem(string id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(todoItem).State = EntityState.Modified; // mark this todo as dirty and to update

        try
        {
            await _context.SaveChangesAsync();
        }
        // SaveChanges for an entity would result in a database update but in fact no rows in the database were affected.
        catch (DbUpdateConcurrencyException)
        {
            bool doesItExist = DoesTodoItemExists(id);
            if (!doesItExist)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }
    
    private bool DoesTodoItemExists(String Id)
    {
        return _context.TodoItems.Any(item => item.Id == Id);
    }

    // DELETE (remove one): api/todo
    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> DeleteTodoItem(string id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NoContent(); // a type of ActionResult
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
