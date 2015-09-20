(function() {
  var bodyHeight, messageTimeout, objHeight, viewportHeight;

  $('label[for="side-menu-open-button"]').click(function() {
    if ($('input#side-menu-open-button').is(':checked')) {
      return $('.sidebar').removeClass('full');
    } else {
      return $('.sidebar').addClass('full');
    }
  });

  window.onscroll = function() {
    var scrolled;
    scrolled = window.pageYOffset || document.documentElement.scrollTop;
    if (scrolled > 5) {
      return $('.header-wrapper').addClass('clipped');
    } else {
      return $('.header-wrapper').removeClass('clipped');
    }
  };

  objHeight = 0;

  if ($('.paper-block')[0] !== void 0) {
    $.each($('paper-block').children(":not(:hidden)"), function() {
      return objHeight += $(this).height();
    });
  }

  bodyHeight = $('body').height();

  viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0;

  $('body').height(Math.max(viewportHeight, objHeight, bodyHeight));

  $('.accordeon > li').click(function() {
    $('.accordeon ul').slideUp();
    if (!$(this).find('ul').is(':visible')) {
      return $(this).find('ul').slideDown();
    }
  });

  messageTimeout = function(message) {
    return setTimeout((function(_this) {
      return function() {
        message.slideUp();
        return setTimeout(function() {
          return message.remove();
        }, 500);
      };
    })(this), 5000);
  };

  $.each($('.temp-message'), function() {
    $(this).slideDown();
    if (!$(this).is('.temp-message-error')) {
      return messageTimeout($(this));
    }
  });

  window.addMessage = function(type, message) {
    var $message, $tempMessagesContainer;
    $tempMessagesContainer = $('.temp-messages');
    $message = $("<div class='temp-message temp-message-" + type + "'> " + message + "</div>");
    $tempMessagesContainer.prepend($message);
    $message.slideDown();
    if (type !== 'error') {
      return messageTimeout($message);
    }
  };

  $.each($('.validation-summary-errors li'), function() {
    return addMessage('error', $(this).html());
  });

  $('#close-messages-button').click(function() {
    return $('.temp-message').slideUp();
  });

  $('body').on('click', '.clickable-row', function() {
    return window.document.location = $(this).data('href');
  });

}).call(this);

//# sourceMappingURL=layout.js.map
