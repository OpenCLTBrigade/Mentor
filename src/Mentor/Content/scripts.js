
window.searched = {};

$(function() {
    $('button.submitSearch').click(function() {
        var addr = $('input[name=Address]').val();
        if (!addr || addr === window.searched.Address) {
            $('form.searchForm').submit();
            return;
        }
        geocoder.geocode({ 'address': addr }, function(results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                window.searched.Address = addr;
                $('input[name=Latitude]').val(results[0].geometry.location.lat());
                $('input[name=Longitude').val(results[0].geometry.location.lng());
                $('form.searchForm').submit();
            }
        });
    });

    $(function() {
        $('[data-toggle="tooltip"]').tooltip().each(function() {
            $(this).css({ outline: 'none' });
        }).click(function() {
            return false;
        });
    });
});
