# CoffeeScript
$('label[for="side-menu-open-button"]').click( ->
    if $('input#side-menu-open-button').is(':checked') 
        $('.sidebar').removeClass('full')
    else 
        $('.sidebar').addClass('full')
)

window.onscroll = ->
    scrolled = window.pageYOffset || document.documentElement.scrollTop
    if scrolled > 5
        $('.header-wrapper').addClass('clipped')
    else $('.header-wrapper').removeClass('clipped')
    
objHeight = 0

if $('.paper-block')[0] isnt undefined
    $.each($('paper-block').children(":not(:hidden)"), ->
        objHeight += $(this).height()
    )

bodyHeight = $('body').height()
viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0
$('body').height(Math.max(viewportHeight, objHeight, bodyHeight))


$('.accordeon > li').click( ->
    $('.accordeon ul').slideUp()
    if !$(this).find('ul').is(':visible')
        $(this).find('ul').slideDown()
)


messageTimeout = (message) ->
    setTimeout( =>
        message.slideUp()
        setTimeout( =>
            message.remove()
        , 500)
    , 5000)

$.each($('.temp-message'), ->
    $(this).slideDown()
    if(!$(this).is('.temp-message-error'))
        messageTimeout($(this))
)

window.addMessage = (type, message) ->
    $tempMessagesContainer = $('.temp-messages')
    $message = $("<div class='temp-message temp-message-#{type}'>
        #{message}</div>")
    $tempMessagesContainer.prepend($message)
    $message.slideDown()
    if (type != 'error')
        messageTimeout($message)
    

$.each($('.validation-summary-errors li'), ->
    addMessage('error', $(this).html())
)      

$('#close-messages-button').click( ->
    $('.temp-message').slideUp()
)

$('body').on('click', '.clickable-row', ->
    window.document.location = $(this).data('href')
)