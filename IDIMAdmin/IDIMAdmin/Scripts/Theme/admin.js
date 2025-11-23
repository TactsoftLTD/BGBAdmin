jQuery(function ($) {

    'use strict';

    var CMPLTADMIN_SETTINGS = window.CMPLTADMIN_SETTINGS || {};

    var m;
    var a;
    /*--------------------------------
        Morris 
     --------------------------------*/
    CMPLTADMIN_SETTINGS.dbMorrisChart = function () {

        /*Bar Graph*/
        // Use Morris.Bar
        m = Morris.Bar({
            element: 'menu_graph',
            resize: true,
            redraw: true,
            xkey: 'ApplicationCode',
            ykeys: ['Menu'],
            labels: ['Menu'],
            barColors: ['#b23c40']
        }).on('click', function (i, row) {
            console.log(i, row);
        });

        /*Area Graph*/
        // Use Morris.Area instead of Morris.Line
        a = Morris.Area({
            element: 'user_graph',
            resize: true,
            redraw: true,
            parseTime: false,
            xkey: 'ApplicationCode',
            ykeys: ['User'],
            labels: ['User'],
            lineColors: ['#b23c40'],
            pointFillColors: ['#E91E63']
        }).on('click', function (i, row) {
            console.log(i, row);
            });
    };

    $(document).ready(function () {
        //console.log(CMPLTADMIN_SETTINGS.dbMorrisChart.data);
        CMPLTADMIN_SETTINGS.dbMorrisChart();
        $.ajax({
            type: "Get",
            url: baseUrl +"/dashboard/application",
            dataType: "json",
            success: function (response) {
                m.setData(response.data);
                a.setData(response.data);
            }
        });
    });

    $(window).load(function () { });

});
