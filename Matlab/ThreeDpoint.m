function point = ThreeDpoint(item,sub_point,M)

    x = M(sub_point{item},1);
    y = M(sub_point{item},2);
    z = M(sub_point{item},3);
    
    point = [x y z];
end

