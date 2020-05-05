$(function() {
    var currentState;

    var semaphoreClosed = false;
    var smartDisplayHasChanges = function() {
        if (semaphoreClosed)
            return;
        semaphoreClosed = true;
        $.ajax({
            url:  "/api/Sitecore/SmartDisplay/HasChanges?currentState="+ currentState,
            type: "POST",
            dataType: "json",
            success: function (data) {
                var hasChanges = data;
                if (hasChanges) {
                    location.reload();
                }
                semaphoreClosed = false;
            },
            async: true
        });
    }

    $(".smart-display").show(400,function(){
        if ($(this).attr("pageMode") === "ExperienceEditor")
            return;
        currentState = $(this).attr("currentState");
        window.setInterval(smartDisplayHasChanges, 5000);
    });
});