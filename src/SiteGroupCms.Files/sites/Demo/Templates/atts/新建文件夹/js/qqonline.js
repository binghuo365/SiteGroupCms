var floatingMenuId = 'qqonlineDiv';
var floatingMenu =
{
    targetX: -170,
    targetY: 80,

    hasInner: typeof(window.innerWidth) == 'number',
    hasElement: document.documentElement
        && document.documentElement.clientWidth,

    menu:
        document.getElementById
        ? document.getElementById(floatingMenuId)
        : document.all
          ? document.all[floatingMenuId]
          : document.layers[floatingMenuId]
};

floatingMenu.move = function ()
{
    if (document.layers)
    {
        floatingMenu.menu.left = floatingMenu.nextX;
        floatingMenu.menu.top = floatingMenu.nextY;
    }
    else
    {
        floatingMenu.menu.style.left = floatingMenu.nextX + 'px';
        floatingMenu.menu.style.top = floatingMenu.nextY + 'px';
    }
}

floatingMenu.computeShifts = function ()
{
    var de = document.documentElement;

    floatingMenu.shiftX =
        floatingMenu.hasInner
        ? pageXOffset
        : floatingMenu.hasElement
          ? de.scrollLeft
          : document.body.scrollLeft;
    if (floatingMenu.targetX < 0)
    {
        if (floatingMenu.hasElement && floatingMenu.hasInner)
        {
            // Handle Opera 8 problems
            floatingMenu.shiftX +=
                de.clientWidth > window.innerWidth
                ? window.innerWidth
                : de.clientWidth
        }
        else
        {
            floatingMenu.shiftX +=
                floatingMenu.hasElement
                ? de.clientWidth
                : floatingMenu.hasInner
                  ? window.innerWidth
                  : document.body.clientWidth;
        }
    }

    floatingMenu.shiftY =
        floatingMenu.hasInner
        ? pageYOffset
        : floatingMenu.hasElement
          ? de.scrollTop
          : document.body.scrollTop;
    if (floatingMenu.targetY < 0)
    {
        if (floatingMenu.hasElement && floatingMenu.hasInner)
        {
            // Handle Opera 8 problems
            floatingMenu.shiftY +=
                de.clientHeight > window.innerHeight
                ? window.innerHeight
                : de.clientHeight
        }
        else
        {
            floatingMenu.shiftY +=
                floatingMenu.hasElement
                ? document.documentElement.clientHeight
                : floatingMenu.hasInner
                  ? window.innerHeight
                  : document.body.clientHeight;
        }
    }
}

floatingMenu.doFloat = function()
{
    var stepX, stepY;

    floatingMenu.computeShifts();

    stepX = (floatingMenu.shiftX +
        floatingMenu.targetX - floatingMenu.nextX) * .07;
    if (Math.abs(stepX) < .0)
    {
        stepX = floatingMenu.shiftX +
            floatingMenu.targetX - floatingMenu.nextX;
    }

    stepY = (floatingMenu.shiftY +
        floatingMenu.targetY - floatingMenu.nextY) * .07;
    if (Math.abs(stepY) < .0)
    {
        stepY = floatingMenu.shiftY +
            floatingMenu.targetY - floatingMenu.nextY;
    }

    if (Math.abs(stepX) > 0 ||
        Math.abs(stepY) > 0)
    {
        floatingMenu.nextX += stepX;
        floatingMenu.nextY += stepY;
        floatingMenu.move();
    }

    setTimeout('floatingMenu.doFloat()', 20);
};
// Download by http://www.codefans.net
// addEvent designed by Aaron Moore
floatingMenu.addEvent = function(element, listener, handler)
{
    if(typeof element[listener] != 'function' ||
       typeof element[listener + '_num'] == 'undefined')
    {
        element[listener + '_num'] = 0;
        if (typeof element[listener] == 'function')
        {
            element[listener + 0] = element[listener];
            element[listener + '_num']++;
        }
        element[listener] = function(e)
        {
            var r = true;
            e = (e) ? e : window.event;

            for(var i = element[listener + '_num'] -1; i >= 0; i--)
            {
                if(element[listener + i](e) == false)
                    r = false;
            }
            return r;
        }
    }

    //if handler is not already stored, assign it
    for(var i = 0; i < element[listener + '_num']; i++)
        if(element[listener + i] == handler)
            return;
    element[listener + element[listener + '_num']] = handler;
    element[listener + '_num']++;
};

floatingMenu.init = function()
{
    floatingMenu.initSecondary();
    floatingMenu.doFloat();
};

// Some browsers init scrollbars only after
// full document load.
floatingMenu.initSecondary = function()
{
    floatingMenu.computeShifts();
    floatingMenu.nextX = floatingMenu.shiftX +
        floatingMenu.targetX;
    floatingMenu.nextY = floatingMenu.shiftY +
        floatingMenu.targetY;
    floatingMenu.move();
}

if (document.layers)
    floatingMenu.addEvent(window, 'onload', floatingMenu.init);
else
{
    floatingMenu.init();
    floatingMenu.addEvent(window, 'onload',
        floatingMenu.initSecondary);
}