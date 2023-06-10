import pymssql
import copy
import random
import heapq
import sys
import numpy as np
import pprint as pp
import pandas as pd 
from matplotlib import pyplot as plt
from mpl_toolkits.mplot3d import Axes3D # 空间三维画图
from sklearn.feature_extraction import DictVectorizer


def Getpd():
    db = pymssql.connect(server='localhost',user='sa',password='123456',database='NewApplication')
    cursor = db.cursor(as_dict=True)
    cursor.execute("SELECT ID,UserId,NewId,Recommendation FROM Recommend")
    data_dict = cursor.fetchall()
    dictvectorizer = DictVectorizer(sparse=False)
    features = dictvectorizer.fit_transform(data_dict)

    UserId = np.arange(0,int(features[len(features[:,0])-1:,3]))
    NewId = np.arange(1,int(features[len(features[:,0])-1:,1])+1)
    Recomm = np.reshape(features[:,2],(len(UserId),len(NewId)))
    df = pd.DataFrame(Recomm,columns=NewId)
    return df



#calculate the kl-distance of c1 and c2
def ChooseColumn(c1,c2):
    # c1 and c2 are the same 
    if c1 == c2:
        return 0
    else:
        c1_li = df[c1].values.tolist()
        c2_li = df[c2].values.tolist()
        #calculate the kl distance
        #find the biggest numbber in the list to determine the loop
        m = max(max(c1_li),max(c2_li))
        #calculate the #i and #j in the kl distance
        i_0 = c1_li.count(0)
        j_0 = c2_li.count(0)
        total_num = column
        result = 0
        for h in range(1,int(m+1)):
            #如果一个向量中的全部值都没有评分
            if (total_num - i_0) == 0 or (total_num - j_0) == 0 or ((total_num - i_0) + (total_num - j_0)) == 0:
                result = 1
                break
            if i_0 == column and j_0 == column:
                result = 1
                break
            #当前评分的数量除有评分的数量
            kl_c1 = c1_li.count(h) / (total_num - i_0)
            kl_c2 = c2_li.count(h) / (total_num - j_0)
            kl_x = (total_num - i_0) / ((total_num - i_0) + (total_num - j_0))
            if kl_c1 == 0 or kl_c2 == 0:
                continue
            #If the denominator of log2 is 0, set kl-distance to 999, which is infinite.
            if ((1 - kl_x) * kl_c2) == 0:
                result = 1
            #calculate the kl distance
            result += kl_x * kl_c1 * np.log2(round(kl_x * kl_c1 / ((1 - kl_x) * kl_c2) ,2))
        if result < 0:
            result = -result
        
        return result


def FindCnei(result_list, n, item):
    Cluster_i = -1
    Cnei_list = []
    
    for i, cluster in enumerate(result_list):
        if item in cluster:
            Cluster_i = i
            break
    
    if Cluster_i == -1:
        print("The input is wrong")
        return 0
    
    if len(result_list[Cluster_i]) <= n:
        return result_list[Cluster_i]
    
    r = []
    for i in result_list[Cluster_i]:
        if item == i:
            r.append(float('inf'))
        else:
            r.append(KL_df.iloc[item-1, i-1])
    
    m = heapq.nsmallest(n, range(len(r)), r.__getitem__)
    Cnei_list = [result_list[Cluster_i][i] for i in m]
    
    return Cnei_list
#calculate the average of the Cnei group
def calculateAve_Cnei(Cnei):
    ave_Cnei = 0
    header_indices = {header: idx for idx, header in enumerate(excel_header)}
    columns_to_average = [header_indices[header] + 1 for header in Cnei if header in header_indices]
    if columns_to_average:
        ave_Cnei = np.mean(df.iloc[:, columns_to_average], axis=1)
    return round(ave_Cnei.mean(), 2) if not np.isnan(ave_Cnei.mean()) else 0

#预测n号用户没有评分的项目       
def ScorePredict(result_list, n):
    for i in range(column):
        if i == n:
            for j in range(line):
                r = 0
                ave_Cnei = 0
                Cnei = []
                if df.iloc[i, j] == 0:
                    Cnei = FindCnei(result_list, n, excel_header[j])
                    ave_Cnei = calculateAve_Cnei(Cnei)
                    j1 = 0
                    j2 = 0
                    for m in Cnei:
                        if j != 0:
                            kl_value = KL_df.iloc[m - 1, excel_header[j] - 1]
                            j1 += 1 / (1 + kl_value) * (np.mean(df, axis=0)[j] - ave_Cnei)
                            j2 += 1 / (1 + kl_value)
                    if j1 == 0 or j2 == 0:
                        r = 0
                    else:
                        r = round((np.mean(df, axis=1)[i] + j1 / j2), 2)
                        if r < 0:
                            r = 0
                    df.iat[i, j] = r
                else:
                    if j >= 5 and j <= 915:
                        for m in range(5):
                            df.iat[i, j - m] += 1


def Topnrecommendation(n,number):
    df_user = df_copy.iloc[n-1:n]
    empty_item = df_user.loc[:,(df_user == 0).any()].columns
    # print(empty_item)
    item = (df.loc[n-1][empty_item]).sort_values(ascending = False,inplace = False)
    print("Recommendation list: ",item.head(number).index.tolist())
    recommendation_list = item.head(number).index.tolist()
    df_recommendation = pd.DataFrame(recommendation_list, columns=["Recommendation"])
    df_recommendation.to_csv("D:/Desktop/alg/try.csv", index=False)



if __name__ == '__main__':
    df = Getpd()
    KL_df = pd.read_csv("D:/Desktop/alg/try1.csv")

    excel_header = df.columns.tolist()
    df.fillna(0, inplace=True)
    column = df.shape[0]
    line = df.shape[1]

    df_copy = Getpd()
    df_copy.fillna(0, inplace=True)
    result_list = [[14, 19, 21, 23, 24, 25, 26, 28, 29, 30, 32, 33, 35, 36, 38, 40, 42, 43, 44, 46, 47, 49, 50, 51, 52, 53, 55, 56, 58, 60, 63, 64, 66, 67, 68, 69, 70, 72, 73, 75, 77, 80, 81, 82, 84, 85, 87, 88, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 101, 102, 106, 107, 114, 116, 117, 118, 119, 120, 122, 124, 125, 126, 129, 130, 131, 132, 134, 136, 138, 139, 140, 141, 143, 145, 147, 148, 149, 150, 151, 152, 153, 154, 155, 158, 160, 161, 162, 164, 165, 166, 167, 168, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 181, 182, 183, 185, 186, 188, 189, 190, 191, 192, 194, 195, 197, 199, 200, 201, 202, 204, 205, 206, 207, 208, 209, 210, 212, 213, 214, 215, 217, 220, 223, 224, 226, 229, 230, 231, 232, 235, 236, 237, 238, 239, 240, 241, 242, 244, 245, 247, 248, 249, 250, 252, 254, 255, 256, 257, 258, 259, 261, 262, 263, 264, 265, 267, 269, 271, 272, 273, 274, 277, 278, 279, 280, 281, 282, 284, 285, 286, 287, 289, 290, 291, 292, 293, 295, 297, 298, 299, 300, 303, 304, 305, 307, 308, 309, 310, 312, 313, 314, 316, 318, 319, 320, 321, 322, 323, 325, 327, 328, 330, 333, 334, 338, 340, 341, 343, 344, 345, 347, 348, 349, 351, 352, 353, 355, 358, 359, 361, 362, 363, 366, 367, 368, 369, 370, 371, 373, 374, 375, 377, 378, 379, 381, 382, 383, 384, 385, 388, 389, 390, 391, 392, 394, 396, 397, 398, 400, 401, 402, 404, 405, 406, 407, 409, 411, 412, 413, 414, 415, 416, 418, 419, 420, 423, 424, 425, 427, 428, 429, 431, 433, 434, 435, 436, 437, 439, 440, 441, 442, 445, 446, 447, 448, 449, 451, 452, 454, 455, 456, 457, 458, 459, 460, 461, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541, 542, 543, 544, 545, 546, 547, 548, 549, 550, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 567, 570, 572, 576, 578, 580, 583, 587, 590, 591, 593, 595, 596, 598, 599, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 775, 776, 777, 778, 779, 780, 782, 784, 786, 788, 790, 792, 793, 795, 797, 798, 799, 801, 802, 803, 805, 808, 809, 810, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827, 828, 829, 830, 831, 832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875, 876, 877, 878, 879, 880, 881, 882, 883, 884, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 898, 899, 900, 901, 902, 903, 904, 905, 906, 907, 908, 909, 910, 911, 912, 913, 914, 915], [83, 12, 13, 27, 34, 642, 674, 679, 689, 691, 693, 694, 696, 726, 728, 729, 731, 735, 744], [6], [37, 1, 4, 5, 7, 8, 10, 11, 15, 17, 20, 48, 54, 62, 71, 74, 76, 78, 79, 103, 105, 108, 109, 110, 111, 112, 113, 115, 121, 123, 127, 128, 133, 135, 137, 142, 144, 146, 156, 157, 159, 163, 169, 180, 184, 187, 196, 198, 203, 211, 216, 218, 219, 221, 222, 225, 227, 228, 233, 234, 243, 246, 251, 253, 260, 266, 268, 270, 275, 276, 283, 288, 294, 296, 301, 302, 306, 311, 315, 317, 324, 326, 329, 331, 332, 335, 336, 337, 339, 342, 346, 350, 354, 356, 357, 360, 364, 365, 372, 376, 380, 386, 387, 393, 395, 399, 403, 408, 410, 417, 421, 422, 426, 430, 432, 438, 443, 444, 450, 453, 462, 609, 610, 612, 614, 616, 617, 618, 621, 627, 629, 634, 638, 644, 646, 647, 657, 669, 671, 734, 736, 781, 785, 806, 807], [9, 2, 3, 16, 18, 22, 31, 39, 41, 45, 57, 59, 61, 65, 86, 89, 100, 104, 193, 566, 568, 569, 571, 573, 574, 575, 577, 579, 581, 582, 584, 585, 586, 588, 589, 592, 594, 597, 600, 601, 602, 603, 604, 605, 606, 607, 608, 611, 613, 615, 619, 620, 622, 623, 624, 625, 626, 628, 630, 631, 632, 633, 635, 636, 637, 639, 640, 641, 643, 645, 648, 649, 650, 651, 652, 653, 654, 655, 656, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 670, 672, 673, 675, 676, 677, 678, 680, 681, 682, 683, 684, 685, 686, 687, 688, 690, 692, 695, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 725, 727, 730, 732, 733, 737, 738, 739, 740, 741, 742, 743, 745, 746, 747, 748, 749, 750, 783, 787, 789, 791, 794, 796, 800, 804, 811]]
    # print("result_list=",result_list)

    ScorePredict(result_list,int(sys.argv[1])-1)
    Topnrecommendation(int(sys.argv[1]),40)
    input()