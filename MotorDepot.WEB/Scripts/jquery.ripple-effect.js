jQuery.fn.clickRippleEffect = function(options){
    var options = options || {};
    var color = options.color || "rgba(0,0,0,.1)";
    var autoChangeElement = options.autoChangeElement || true;

    $item = $(this);

    $item.click(function(e) {

        $this = $(this);

        if ($this.find('.ink').length == 0)
            $this.prepend("<span class='ink'></span>");

        var $ink = $this.find('.ink');
        $ink.removeClass('animate');

        if (!$ink.height() && !$ink.width()) {
            var d = Math.max($this.outerWidth(), $this.outerHeight());
            $ink.css({height: d, width: d, background: color});
        }

        if (autoChangeElement)
        {
            $this.css({position: 'relative', overflow: 'hidden'});
        }

        var x = e.pageX - $this.offset().left - $ink.width()/2;
        var y = e.pageY - $this.offset().top - $ink.height()/2;

        $ink.css({
            top: y + 'px',
            left: x + 'px'
        }).addClass('animate');
    });
};