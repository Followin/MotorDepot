isEmpty = (el) -> !$.trim(el.html())

modalOpened = false
modal = null
animating = false

$('body').on('click', '.more-info-button', (e)->
    return if animating or modalOpened
    animating = true
    $container = $(e.target).closest('.info')
    $fullInfo = $("<div class='paper-block vehicle-info modal minified'></div>")
    vehicleId = $container.find("input[type='hidden'].model_id").val()
    
    $.ajax(
        type: "GET"
        url: "/Vehicle/FullInfo/#{vehicleId}"
        success: (data, statusText) ->
            if data != ""
                $fullInfo.append(data)
                $('body').append($fullInfo)
                $('#main-content').addClass('modal-opened')
                modalOpened = true
                $fullInfo.focus()
                modal = $fullInfo
                $('body').css('overflow', 'hidden')
                setTimeout( -> 
                    $fullInfo.removeClass('minified')
                    animating = false
                , 0)
    )
    
)

$('body').on('click', (e)->
    return if animating
    
    target = e.target
    while target != e.delegateTarget
        return if modal isnt null and target == modal[0]
        target = target.parentNode
    if target == e.delegateTarget
        if modalOpened
            animating = true
            $(modal).addClass('minified')
            $(modal).fadeOut()
            setTimeout( ->
                $(modal).remove()
                $('body').css('overflow', '')
                modal = null
                animating = false
            , 300)
            $('#main-content').removeClass('modal-opened')
            modalOpened = false
)
