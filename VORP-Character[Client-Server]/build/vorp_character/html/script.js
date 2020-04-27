$(function() {
    window.addEventListener('message', function(event) {
        if (event.data.type == "enableui") {
            document.body.style.display = event.data.enable ? "block" : "none";
        }
    });

    document.onkeyup = function(data) {
        if (data.which == 27) { // Escape key
            $.post('http://vorp_character/escape', JSON.stringify({}));
        }
    };

    $("#register").submit(function(event) {
        //event.preventDefault(); // Prevent form from submitting

        $.post('http://vorp_character/register', JSON.stringify({
            firstname: $("#nombrerol").val()
        }));
    });
});