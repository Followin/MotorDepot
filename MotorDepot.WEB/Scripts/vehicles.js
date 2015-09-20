(function() {
  var animating, isEmpty, modal, modalOpened;

  isEmpty = function(el) {
    return !$.trim(el.html());
  };

  modalOpened = false;

  modal = null;

  animating = false;

  $('body').on('click', '.more-info-button', function(e) {
    var $container, $fullInfo, vehicleId;
    if (animating || modalOpened) {
      return;
    }
    animating = true;
    $container = $(e.target).closest('.info');
    $fullInfo = $("<div class='paper-block vehicle-info modal minified'></div>");
    vehicleId = $container.find("input[type='hidden'].model_id").val();
    return $.ajax({
      type: "GET",
      url: "/Vehicle/FullInfo/" + vehicleId,
      success: function(data, statusText) {
        if (data !== "") {
          $fullInfo.append(data);
          $('body').append($fullInfo);
          $('#main-content').addClass('modal-opened');
          modalOpened = true;
          $fullInfo.focus();
          modal = $fullInfo;
          $('body').css('overflow', 'hidden');
          return setTimeout(function() {
            $fullInfo.removeClass('minified');
            return animating = false;
          }, 0);
        }
      }
    });
  });

  $('body').on('click', function(e) {
    var target;
    if (animating) {
      return;
    }
    target = e.target;
    while (target !== e.delegateTarget) {
      if (modal !== null && target === modal[0]) {
        return;
      }
      target = target.parentNode;
    }
    if (target === e.delegateTarget) {
      if (modalOpened) {
        animating = true;
        $(modal).addClass('minified');
        $(modal).fadeOut();
        setTimeout(function() {
          $(modal).remove();
          $('body').css('overflow', '');
          modal = null;
          return animating = false;
        }, 300);
        $('#main-content').removeClass('modal-opened');
        return modalOpened = false;
      }
    }
  });

}).call(this);

//# sourceMappingURL=vehicles.js.map
