using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoBackend;
using TodoBackend.controller;
using TodoBackend.models;

namespace TodoTests;

[TestClass]
public sealed class TodoControllerTest
{
    TodoItem[] mockData =
    [
        new TodoItem{Id = "1", Description = "Activity", Done = false}, 
        new TodoItem{Id = "2", Description = "Activity 2", Done = true}
    ];
    
    [TestMethod]
    public async Task Should_GetTodoItems_Returns_Two_Count()
    {
        // asserting that first calling get todos will return the initial seed results 
        var mockContext = new Mock<TodoContext>();
        TodoController controller = new TodoController(mockContext.Object);
        ActionResult<IEnumerable<TodoItem>> result = await controller.GetTodoItems();
        Assert.AreEqual(result?.Value?.Count(), 2);
    }
    
    [TestMethod]
    public async Task Should_DeleteTodoItem_Returns_One_Count()
    {
        // asserting that using the first id of the list of test data and calling deleteTodoItem
        // will change the number of TodoItems from 2 to 1
        var mockContext = new Mock<TodoContext>();
        TodoController controller = new TodoController(mockContext.Object);
        ActionResult<IEnumerable<TodoItem>> result = await controller.GetTodoItems();
        TodoItem? firstTodo = result?.Value?.First();
        await controller.DeleteTodoItem(firstTodo?.Id);
        Assert.AreEqual(result?.Value?.Count(), 1);
    }

    [TestMethod]
    public async Task Test_AddTodoItem_Returns_One_Item()
    {
        // asserting that by adding a new todo item to the list of test data
        // will return the Id of the new todo
        // total number of todoItems (array length) will increment by 1
        var mockContext = new Mock<TodoContext>();
        TodoController controller = new TodoController(mockContext.Object);
        TodoItem newTodo = new TodoItem { Id = "3", Description = "something", Done = false };
        var postResponse = await controller.PostTodoItem(newTodo);
        Assert.AreEqual(postResponse?.Value?.Id, "3");
        ActionResult<IEnumerable<TodoItem>> allTodos = await controller.GetTodoItems();
        Assert.AreEqual(allTodos?.Value?.Count(), 3);
    }
}