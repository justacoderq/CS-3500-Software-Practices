
window.initRenderJS = (instance) => {
    window.theInstance = instance;
};

document.addEventListener('keydown', function (event)
{
    // Optionally log the key for testing
    console.log('Key pressed:', event.key);

    // Call the C# method and pass the key pressed
    theInstance.invokeMethodAsync('HandleKeyPress', event.key);

});
