using EventScheduler.Services;
using EventScheduler.UI;

var eventService = new EventService();
var consoleUI = new ConsoleUI(eventService);
consoleUI.Run();
